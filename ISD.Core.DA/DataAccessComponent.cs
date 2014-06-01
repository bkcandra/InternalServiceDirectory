using System.Data.Entity;
using BCUtility;
using ISD.Data.EDM;
using ISD.EDS;
using ISD.Util;
using nullpointer.Metaphone;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISD.DA
{
    public class DataAccessComponent
    {
        private ISDEntities ent =new ISDEntities();

        #region Category
        public DataSetComponent.CategoryDataTable RetrieveCategories()
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.CategoryDataTable dt = new DataSetComponent.CategoryDataTable();

            var query = from c in ent.v_CategoryExplorer
                        select c;

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                // ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt);
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public int DetermineCategoryLevel(DataSetComponent.v_CategoryExplorerRow categoryDR)
        {
            if (categoryDR.IsLevel1ParentIDNull() || categoryDR.Level1ParentID == 0)
                return 0;
            else if (categoryDR.IsLevel2ParentIDNull() || categoryDR.Level2ParentID == 0)
                return 1;
            else
                return 2;

        }

        public DataSetComponent.v_CategoryExplorerRow RetrieveCategoryView(int categoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.v_CategoryExplorer
                        where c.ID == categoryID
                        select c;

            var category = query.SingleOrDefault();

            if (category == null)
                return null;
            else
            {
                var dr = new DataSetComponent.v_CategoryExplorerDataTable().Newv_CategoryExplorerRow();
                ObjectHandler.CopyPropertyValues(category, dr);
                return dr;
            }
        }


        public DataSetComponent.v_CategoryExplorerDataTable RetrieveCategories(int startingRef)
        {
            ISDEntities ent = new ISDEntities();

            DataSetComponent.v_CategoryExplorerRow startPoint = RetrieveCategoryView(startingRef);

            IQueryable<v_CategoryExplorer> query = null;

            if (startPoint == null)
            {
                query = from b in ent.v_CategoryExplorer
                        orderby b.Level2ParentName, b.Level1ParentName, b.Name
                        select b;
            }
            else
            {
                query = from b in ent.v_CategoryExplorer
                        where
                         (startPoint.Level == 0 && b.Level1ParentID == startingRef) ||
                         (startPoint.Level == 1 && b.Level1ParentID == startPoint.Level1ParentID && b.Level2ParentID == startingRef)
                        orderby b.Level2ParentName, b.Level1ParentName, b.Name
                        select b;
            }

            var dt = new DataSetComponent.v_CategoryExplorerDataTable();

            ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
            return dt;
        }

        public int DetermineCategoryLevel(DataSetComponent.CategoryRow categoryDR)
        {
            if (categoryDR.IsLevel1ParentIDNull() || categoryDR.Level1ParentID == 0)
                return 0;
            else if (categoryDR.IsLevel2ParentIDNull() || categoryDR.Level2ParentID == 0)
                return 1;
            else
                return 2;
        }

        public void CreateCategory(DataSetComponent.CategoryRow dr)
        {
            ISDEntities ent = new ISDEntities();
            Category cat = new Category();
            ObjectHandler.CopyPropertyValues(dr, cat);
            ent.Category.Add(cat);
            ent.SaveChanges();
        }

        public void UpdateCategory(DataSetComponent.CategoryRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Category
                        where c.ID == dr.ID
                        select c;

            Category cat = query.FirstOrDefault();

            if (cat != null)
            {
                cat.Name = dr.Name;
                cat.Level1ParentID = dr.Level1ParentID;
                cat.Level2ParentID = dr.Level2ParentID;
                cat.CreatedBy = dr.CreatedBy;
                cat.CreatedDateTime = dr.CreatedDateTime;
                cat.ModifiedBy = dr.ModifiedBy;
                cat.ModifiedDateTime = dr.ModifiedDateTime;
            }

            ent.SaveChanges();
        }

        public void UpdateCategoryName(DataSetComponent.CategoryRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Category
                        where c.ID == dr.ID
                        select c;

            Category cat = query.FirstOrDefault();

            if (cat != null)
            {
                cat.Name = dr.Name;
                cat.ModifiedBy = dr.ModifiedBy;
                cat.ModifiedDateTime = dr.ModifiedDateTime;
            }

            ent.SaveChanges();
        }

        public void DeleteCategory(int categoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Category
                        where c.ID == categoryID
                        select c;

            Category cat = query.FirstOrDefault();
            if (cat != null)
            {
                ent.Category.Remove(cat);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.CategoryRow RetrieveCategory(int categoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.v_CategoryExplorer
                        where c.ID == categoryID
                        select c;

            var dr = new DataSetComponent.CategoryDataTable().NewCategoryRow();
            var category = query.SingleOrDefault();

            if (category == null) return null;
            else
            {
                ObjectHandler.CopyPropertyValues(category, dr);
                return dr;
            }
        }

        public DataSetComponent.CategoryDataTable RetrieveAllCategories()
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.CategoryDataTable dt = new DataSetComponent.CategoryDataTable();

            var query = from c in ent.v_CategoryExplorer
                        select c;

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                // ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt);
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public DataSetComponent.CategoryDataTable RetrieveSubCategories(int immediateParentCategoryID, int startIndex, int amount, string sortExpression)
        {
            IQueryable<v_CategoryExplorer> query = null;
            ISDEntities ent = new ISDEntities();
            sortExpression = "c." + sortExpression;
            int parentLevel = 0;

            if (immediateParentCategoryID != 0)
            {
                var parent = RetrieveCategory(immediateParentCategoryID);
                parentLevel = DetermineCategoryLevel(parent);
            }

            switch (parentLevel)
            {
                case 0:
                    query = from c in ent.v_CategoryExplorer
                            where c.Level1ParentID == immediateParentCategoryID && c.Level2ParentID == 0
                            orderby sortExpression
                            select c;
                    break;
                case 1:
                    query = from c in ent.v_CategoryExplorer
                            where c.Level2ParentID == immediateParentCategoryID
                            orderby sortExpression
                            select c;
                    break;
            }

            DataSetComponent.CategoryDataTable dt = new DataSetComponent.CategoryDataTable();

            if (query != null)
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
            }

            return dt;
        }

        public int RetrieveSubCategoriesCount(int immediateParentCategoryID, string sortExpression)
        {
            IQueryable<v_CategoryExplorer> query = null;
            ISDEntities ent = new ISDEntities();

            int parentLevel = 0;

            if (immediateParentCategoryID != 0)
            {
                var parent = RetrieveCategory(immediateParentCategoryID);
                parentLevel = DetermineCategoryLevel(parent);
            }

            switch (parentLevel)
            {
                case 0:
                    query = from c in ent.v_CategoryExplorer
                            where c.Level1ParentID == immediateParentCategoryID && c.Level2ParentID == 0
                            select c;
                    break;
                case 1:
                    query = from c in ent.v_CategoryExplorer
                            where c.Level2ParentID == immediateParentCategoryID

                            select c;
                    break;
                case 2:
                    query = from c in ent.v_CategoryExplorer
                            where c.ID == immediateParentCategoryID

                            select c;
                    break;
            }

            return query.Count();
        }

        public DataSetComponent.CategoryDataTable RetrieveAllSubCategories(int parentCategoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.v_CategoryExplorer
                        where c.Level1ParentID == parentCategoryID ||
                                c.Level2ParentID == parentCategoryID

                        select c;

            var dt = new DataSetComponent.CategoryDataTable();

            ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);

            return dt;
        }



        public int RetrieveCategoriesCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Category
                        select c;

            return query.Count();
        }

        public string RetrieveLastCategoryID()
        {
            ISDEntities ent = new ISDEntities();
            var query = from c in ent.Category
                        orderby c.ID descending
                        select c.ID;

            string categoryID = "";
            if (query.Count() != 0)
            {
                categoryID = query.FirstOrDefault().ToString();
            }
            return categoryID;
        }

        public DataSetComponent.CategoryDataTable RetrieveLv0Categories()
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.CategoryDataTable dt = new DataSetComponent.CategoryDataTable();

            var query = from c in ent.v_CategoryExplorer
                        where c.Level == 0
                        select c;

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                // ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt);
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public DataSetComponent.CategoryDataTable RetrieveLv1Categories(int rootCatID)
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.CategoryDataTable dt = new DataSetComponent.CategoryDataTable();

            var query = from c in ent.v_CategoryExplorer
                        where c.Level == 1 && c.Level1ParentID == rootCatID
                        select c;

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                // ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt);
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public int RetrieveCategoryLevel(int CurrentCategoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.v_CategoryExplorer
                        where e.ID == CurrentCategoryID
                        select e;
            var dr = new DataSetComponent.CategoryDataTable().NewCategoryRow();
            var category = query.SingleOrDefault();

            if (category == null)
                return SystemConstants.intError;
            else
            {
                return category.Level;
            }


        }
        #endregion

        #region suburb

        public void DeleteSuburb(int suburbID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Suburb
                        where c.ID == suburbID
                        select c;

            Suburb sub = query.FirstOrDefault();
            if (sub != null)
            {
                ent.Suburb.Remove(sub);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.SuburbDataTable RetrieveSuburbs(int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.SuburbDataTable dt = new DataSetComponent.SuburbDataTable();

            var query = from q in ent.v_SuburbExplorer
                        orderby sortExpression
                        select q;

            if (query.AsEnumerable().Count() == 0)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query.Skip(startIndex).Take(amount).AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public int RetrieveSuburbsCount(string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.SuburbDataTable dt = new DataSetComponent.SuburbDataTable();

            var query = from q in ent.v_SuburbExplorer
                        select q;

            return query.Count();
        }

        public void CreateSuburb(string Modifier, DataSetComponent.SuburbRow dr)
        {
            ISDEntities ent = new ISDEntities();
            Suburb sub = new Suburb();
            sub.Name = dr.Name;
            sub.PostCode = dr.PostCode;
            sub.StateID = dr.StateID;

            ent.Suburb.Add(sub);
            ent.SaveChanges();
        }

        public void UpdateSuburb(string Modifier, DataSetComponent.SuburbRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.Suburb
                        where s.ID == dr.ID
                        select s;

            Suburb sub = query.FirstOrDefault();

            if (sub != null)
            {
                //ObjectHandler.CopyProperties(dr, sub);
                sub.Name = dr.Name;
                sub.PostCode = dr.PostCode;
                sub.StateID = dr.StateID;
            }

            ent.SaveChanges();
        }

        public DataSetComponent.v_SuburbExplorerRow RetrieveSuburb(int suburbID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.v_SuburbExplorer
                        where s.ID == suburbID
                        select s;

            DataSetComponent.v_SuburbExplorerDataTable dt = new DataSetComponent.v_SuburbExplorerDataTable();
            DataSetComponent.v_SuburbExplorerRow dr = new DataSetComponent.v_SuburbExplorerDataTable().Newv_SuburbExplorerRow();


            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }

        public DataSetComponent.v_SuburbExplorerDataTable RetrieveSuburbs()
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.v_SuburbExplorerDataTable dt = new DataSetComponent.v_SuburbExplorerDataTable();

            var query = from q in ent.v_SuburbExplorer
                        select q;


            if (query.AsEnumerable().Count() == 0)
                return null;
            else
            {
                query = query.OrderBy(row => row.Name);
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
        }

        public DataSetComponent.v_SuburbExplorerRow RetrieveSuburbByID(int suburbID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.v_SuburbExplorer
                        where s.ID == suburbID
                        select s;

            DataSetComponent.v_SuburbExplorerDataTable dt = new DataSetComponent.v_SuburbExplorerDataTable();
            DataSetComponent.v_SuburbExplorerRow dr = new DataSetComponent.v_SuburbExplorerDataTable().Newv_SuburbExplorerRow();


            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }

        public DataSetComponent.v_SuburbExplorerRow RetrieveSuburbByPostCode(int postCode)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.v_SuburbExplorer
                        where s.PostCode == postCode
                        select s;

            DataSetComponent.v_SuburbExplorerDataTable dt = new DataSetComponent.v_SuburbExplorerDataTable();
            DataSetComponent.v_SuburbExplorerRow dr = new DataSetComponent.v_SuburbExplorerDataTable().Newv_SuburbExplorerRow();


            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }


        #endregion

        #region state
        public DataSetComponent.StateDataTable RetrieveStates()
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        select s;

            DataSetComponent.StateDataTable dt = new DataSetComponent.StateDataTable();

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);

                //ObjectHandler.CopyProperties(category, dr);
                return dt;
            }
        }

        public DataSetComponent.StateDataTable RetrieveStates(int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        select s;

            DataSetComponent.StateDataTable dt = new DataSetComponent.StateDataTable();

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);

                //ObjectHandler.CopyProperties(category, dr);
                return dt;
            }
        }

        public int RetrieveStatesCount(string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        select s;

            return query.Count();
        }

        public DataSetComponent.StateRow RetrieveState(int stateID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        where s.ID == stateID
                        select s;

            DataSetComponent.StateDataTable dt = new DataSetComponent.StateDataTable();
            DataSetComponent.StateRow dr = new DataSetComponent.StateDataTable().NewStateRow();


            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }

        public void CreateState(string userName, DataSetComponent.StateRow dr)
        {
            ISDEntities ent = new ISDEntities();
            State state = new State();
            state.StateName = dr.StateName;
            state.StateDetail = dr.StateDetail;

            ent.State.Add(state);
            ent.SaveChanges();
        }

        public void UpdateState(string userName, DataSetComponent.StateRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        where s.ID == dr.ID
                        select s;

            State state = query.FirstOrDefault();

            if (state != null)
            {
                //ObjectHandler.CopyProperties(dr, sub);
                state.StateName = dr.StateName;
                state.StateDetail = dr.StateDetail;
            }

            ent.SaveChanges();
        }

        public void DeleteState(int StateID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        where s.ID == StateID
                        select s;

            State sub = query.FirstOrDefault();
            if (sub != null)
            {
                ent.State.Remove(sub);
                ent.SaveChanges();
            }
        }
        #endregion

        #region Member

        public DataSetComponent.UserProfilesRow RetrieveUserProfiles(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from u in ent.UserProfiles
                        where u.UserID == userID
                        select u;

            UserProfiles user = query.FirstOrDefault();
            if (user != null)
            {
                var dr = new DataSetComponent.UserProfilesDataTable().NewUserProfilesRow();
                ObjectHandler.CopyPropertyValues(user, dr);
                return dr;
            }
            else return null;
        }

        public DataSetComponent.v_UserExplorerDataTable RetrieveCustomerList(int startIndex, int amount)
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<v_UserExplorer> query = from c in ent.v_UserExplorer
                                               orderby c.UserName
                                               select c;
            if (amount < 0)
                amount = query.Count();
            var customers = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (customers != null)
            {
                var dt = new DataSetComponent.v_UserExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(customers, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;

        }

        public int RetrieveCustomerListCount()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<v_UserExplorer> query = from c in ent.v_UserExplorer
                                               orderby c.UserName
                                               select c;

            return query.Count();
        }

        public DataSetComponent.UserProfilesDataTable RetrieveUserProfiles()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<UserProfiles> query = from p in ent.UserProfiles
                                             select p;
            var Users = query.AsEnumerable();
            if (Users != null)
            {
                var dt = new DataSetComponent.UserProfilesDataTable();
                ObjectHandler.CopyEnumerableToDataTable(Users, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public void UpdateUserProfiles(DataSetComponent.UserProfilesRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.UserProfiles
                        where ac.UserID == dr.UserID
                        select ac;

            UserProfiles user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(dr, user);
                ent.SaveChanges();
            }
        }

        #endregion

        #region Provider
        public DataSetComponent.ProviderProfilesDataTable RetrieveProviders()
        {
            ISDEntities ent = new ISDEntities();

            var query = ent.ProviderProfiles.AsEnumerable();
            if (query != null)
            {
                var dt = new DataSetComponent.ProviderProfilesDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.v_ProviderExplorerDataTable RetrieveProviderList(int startIndex, int amount)
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<v_ProviderExplorer> query = from p in ent.v_ProviderExplorer
                                                   orderby p.UserName
                                                   select p;
            if (amount < 0)
                amount = query.Count();
            var customers = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (customers != null)
            {
                var dt = new DataSetComponent.v_ProviderExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(customers, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;

        }

        public int RetrieveProviderListCount()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<v_ProviderExplorer> query = from p in ent.v_ProviderExplorer
                                                   orderby p.UserName
                                                   select p;

            return query.Count();
        }

        public DataSetComponent.ProviderProfilesRow RetrieveProviderProfilesByID(String providerID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.ProviderProfiles
                        where p.UserID == providerID
                        select p;
            ProviderProfiles prov = query.FirstOrDefault();
            if (prov != null)
            {
                DataSetComponent.ProviderProfilesRow dr = new DataSetComponent.ProviderProfilesDataTable().NewProviderProfilesRow();
                ObjectHandler.CopyPropertyValues(prov, dr);
                return dr;
            }
            else return null;
        }

        public DataSetComponent.ProviderProfilesRow RetrieveProviderProfiles(String providerUsername)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.ProviderProfiles
                        where p.Username == providerUsername
                        select p;

            var dt = new DataSetComponent.ProviderProfilesDataTable();
            var dr = new DataSetComponent.ProviderProfilesDataTable().NewProviderProfilesRow();

            if (query.SingleOrDefault() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.OverwriteChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }

        public DataSetComponent.ProviderProfilesDataTable RetrieveProviderProfiles()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<ProviderProfiles> query = from p in ent.ProviderProfiles
                                                 select p;
            var providers = query.AsEnumerable();
            if (providers != null)
            {
                var dt = new DataSetComponent.ProviderProfilesDataTable();
                ObjectHandler.CopyEnumerableToDataTable(providers, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public void UpdateProviderProfiles(DataSetComponent.ProviderProfilesRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.ProviderProfiles
                        where ac.UserID == dr.UserID
                        select ac;

            ProviderProfiles user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(dr, user);
                ent.SaveChanges();
            }
        }
        #endregion

        #region KeywordsManagement
        public DataSetComponent.v_KeyCollectionViewDataTable RetrieveKeyCollections(int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.v_KeyCollectionView
                        select s;

            DataSetComponent.v_KeyCollectionViewDataTable dt = new DataSetComponent.v_KeyCollectionViewDataTable();

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);

                //ObjectHandler.CopyProperties(category, dr);
                return dt;
            }
        }

        public DataSetComponent.v_KeyCollectionViewRow RetrieveKeyword(int CollectionID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from kc in ent.v_KeyCollectionView
                        where kc.ID == CollectionID
                        select kc;

            var dt = new DataSetComponent.v_KeyCollectionViewDataTable();
            var dr = new DataSetComponent.v_KeyCollectionViewDataTable().Newv_KeyCollectionViewRow();
            var category = query.SingleOrDefault();

            if (category == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                return dr;
            }
        }

        public int RetrieveKeyCollectionsCount(string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.v_KeyCollectionView
                        select s;

            return query.Count();
        }

        /*public DataSetComponent.StateRow RetrieveKeywords(int keyCollectionID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.State
                        where s.ID == stateID
                        select s;

            DataSetComponent.StateDataTable dt = new DataSetComponent.StateDataTable();
            DataSetComponent.StateRow dr = new DataSetComponent.StateDataTable().NewStateRow();


            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                //ObjectHandler.CopyProperties(category, dr);
                return dr;
            }
        }*/

        public void DeleteKeyCollection(int keyColletionID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from keyColl in ent.KeyCollection
                        where keyColl.ID == keyColletionID
                        select keyColl;

            KeyCollection key = query.FirstOrDefault();
            if (key != null)
            {
                ent.KeyCollection.Remove(key);
                ent.SaveChanges();
            }
        }

        public void CreateKeyCollection(DataSetComponent.KeyCollectionRow drKeyProperties, out int KeyColID)
        {
            ISDEntities ent = new ISDEntities();
            KeyCollection key = new KeyCollection();

            key.Name = drKeyProperties.Name;
            key.Description = drKeyProperties.Description;

            ent.KeyCollection.Add(key);
            ent.SaveChanges();
            KeyColID = key.ID;
        }

        public void createKeywords(DataSetComponent.KeywordRow drKeywords, int KeyColID)
        {
            ISDEntities ent = new ISDEntities();
            Keyword keyword = new Keyword();

            keyword.KeyCollectionID = KeyColID;
            keyword.Keywords = drKeywords.Keywords;

            ent.Keyword.Add(keyword);
            ent.SaveChanges();
        }

        public void UpdateKeyCollection(DataSetComponent.KeyCollectionRow drKeyProperties)
        {
            ISDEntities ent = new ISDEntities();

            var query = from kc in ent.KeyCollection
                        where kc.ID == drKeyProperties.ID
                        select kc;

            KeyCollection keyCollection = query.FirstOrDefault();

            if (keyCollection != null)
            {
                //ObjectHandler.CopyProperties(dr, sub);
                keyCollection.Name = drKeyProperties.Name;
                keyCollection.Description = drKeyProperties.Description;
            }

            ent.SaveChanges();
        }

        public void UpdateKeywords(DataSetComponent.KeywordRow drKeywords)
        {
            ISDEntities ent = new ISDEntities();

            var query = from k in ent.Keyword
                        where k.ID == drKeywords.KeyCollectionID
                        select k;

            Keyword key = query.FirstOrDefault();

            if (key != null)
            {
                //ObjectHandler.CopyProperties(dr, sub);
                key.Keywords = drKeywords.Keywords;
            }

            ent.SaveChanges();
        }
        #endregion

        #region page

        public DataSetComponent.PageRow RetrievePage(string PageName)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.Name == PageName
                        select p;

            Page page = query.FirstOrDefault();

            if (page != null)
            {
                var dr = new DataSetComponent.PageDataTable().NewPageRow(); ;
                ObjectHandler.CopyPropertyValues(page, dr);
                return dr;
            }
            else return null;
        }

        public DataSetComponent.PageDataTable RetrievePages(int PageType, int startIndex, int amount)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.PageType == PageType
                        orderby p.Name
                        select p;

            IEnumerable<Page> page = query.Skip(startIndex).Skip(amount).AsEnumerable();
            if (page != null)
            {
                DataSetComponent.PageDataTable dt = new DataSetComponent.PageDataTable();
                ObjectHandler.CopyEnumerableToDataTable(page, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int RetrievePagesCount(int PageType, int startIndex, int Amount)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.PageType == PageType
                        orderby p.Name
                        select p;

            return query.Count();
        }

        public bool isPageExist(string name)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.Name.ToUpper().Equals(name.ToUpper())
                        select p;

            var pages = query.FirstOrDefault();

            if (pages != null)
            {
                return true;
            }
            else return false;
        }

        public DataSetComponent.PageDataTable RetrievePages(int pageType)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.PageType == pageType
                        select p;

            var pages = query.AsEnumerable();

            if (pages != null)
            {
                var dt = new DataSetComponent.PageDataTable();
                ObjectHandler.CopyEnumerableToDataTable(pages, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.PageRow RetrievePage(int pageID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.ID == pageID
                        select p;

            var page = query.FirstOrDefault();

            if (page != null)
            {
                var dr = new DataSetComponent.PageDataTable().NewPageRow(); ;
                ObjectHandler.CopyPropertyValues(page, dr);
                return dr;
            }
            else return null;

        }

        public void DeletePage(int PageID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.ID == PageID
                        select p;

            Page page = query.FirstOrDefault();
            if (page != null)
            {
                ent.Page.Remove(page);
                ent.SaveChanges();
            }
        }

        public void CreatePage(DataSetComponent.PageRow dr)
        {
            ISDEntities ent = new ISDEntities();

            Page page = new Page();
            ObjectHandler.CopyPropertyValues(dr, page);
            ent.Page.Add(page);
            ent.SaveChanges();
            dr.ID = page.ID;
        }

        public void UpdatePage(DataSetComponent.PageRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        where p.ID == dr.ID
                        select p;

            var page = query.FirstOrDefault();

            if (page != null)
            {
                ObjectHandler.CopyPropertyValues(dr, page);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.PageDataTable RetrievePages()
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Page
                        select p;

            var pages = query.AsEnumerable();

            if (pages != null)
            {
                var dt = new DataSetComponent.PageDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        #endregion

        #region MailTemplate

        public DataSetComponent.EmailTemplateDataTable RetrieveMailTemplates(int EmailType, int startIndex, int amount)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        where p.EmailType == EmailType
                        orderby p.EmailName
                        select p;

            IEnumerable<EmailTemplate> emails = query.Skip(startIndex).Skip(amount).AsEnumerable();
            if (emails != null)
            {
                DataSetComponent.EmailTemplateDataTable dt = new DataSetComponent.EmailTemplateDataTable();
                ObjectHandler.CopyEnumerableToDataTable(emails, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int RetrieveMailTemplatesCount(int EmailType, int startIndex, int Amount)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        where p.EmailType == EmailType
                        orderby p.EmailName
                        select p;

            return query.Count();
        }

        public DataSetComponent.EmailTemplateDataTable RetrieveEmailTemplates()
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        select p;

            var templates = query.AsEnumerable();

            if (templates != null)
            {
                var dt = new DataSetComponent.EmailTemplateDataTable();
                ObjectHandler.CopyEnumerableToDataTable(templates, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.EmailTemplateDataTable RetrieveEmailTemplates(int EmailType)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        where p.EmailType == EmailType
                        select p;

            var templates = query.AsEnumerable();

            if (templates != null)
            {
                var dt = new DataSetComponent.EmailTemplateDataTable();
                ObjectHandler.CopyEnumerableToDataTable(templates, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.EmailTemplateRow RetrieveEmailTemplate(int EmailTemplateID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        where p.ID == EmailTemplateID
                        select p;

            var template = query.FirstOrDefault();

            if (template != null)
            {
                var dr = new DataSetComponent.EmailTemplateDataTable().NewEmailTemplateRow(); ;
                ObjectHandler.CopyPropertyValues(template, dr);
                return dr;
            }
            else return null;

        }

        public void DeleteEmailTemplate(int EmailTemplateID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.EmailTemplate
                        where p.ID == EmailTemplateID
                        select p;

            EmailTemplate template = query.FirstOrDefault();
            if (template != null)
            {
                ent.EmailTemplate.Remove(template);
                ent.SaveChanges();
            }
        }

        public void CreateEmailTemplate(DataSetComponent.EmailTemplateRow dr)
        {
            ISDEntities ent = new ISDEntities();

            EmailTemplate email = new EmailTemplate();
            ObjectHandler.CopyPropertyValues(dr, email);
            ent.EmailTemplate.Add(email);
            ent.SaveChanges();
            dr.ID = email.ID;
        }

        public void UpdateEmailTemplate(DataSetComponent.EmailTemplateRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.EmailTemplate
                        where e.ID == dr.ID
                        select e;

            var email = query.FirstOrDefault();

            if (email != null)
            {
                ObjectHandler.CopyPropertyValues(dr, email);
                ent.SaveChanges();
            }
        }

        #endregion

        #region Activities
        public DataSetComponent.v_ActivityExplorerDataTable RetrieveActivitiesExplorer()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_ActivityExplorer
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.v_VoucherExplorerDataTable RetrieveallVouchers()
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_VoucherExplorer> query = null;

            query = from a in ent.v_VoucherExplorer
                    select a;

            var allVouchers = query.AsEnumerable();
            if (allVouchers != null)
            {
                DataSetComponent.v_VoucherExplorerDataTable dt = new DataSetComponent.v_VoucherExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allVouchers, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;
        }


        public int RetrieveActivitiesExplorerCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_ActivityExplorer
                        select q;

            return query.Count();


        }

        public DataSetComponent.ActivityDataTable RetrieveActivities()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.ActivityDataTable RetrieveActivitiesDontIncludeDeleted()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        where q.Status != (int)SystemConstants.ActivityStatus.Deleted
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.ActivityDataTable RetrievePendingActivities()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        where q.isApproved == false
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int RetrievePendingActivitiesCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        where q.isApproved == false
                        select q;
            return query.Count();
        }

        public int RetrieveProviderActivitiesCount(String ProviderID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        where q.ProviderID == (ProviderID.ToString())
                        select q;

            return query.Count();


        }

        public int RetrieveApprovedActivitiesCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        where q.isApproved == true
                        select q;

            return query.Count();


        }

        public int RetrieveActivitiesCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Activity
                        select q;

            return query.Count();
        }
        public int RetrieveActivitiesCount(int status, bool approved)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<Activity> query;
            if (status == (int)SystemConstants.ActivityStatus.Deleting || status == (int)SystemConstants.ActivityStatus.Expired)
            {
                query = from q in ent.Activity
                        where q.Status == status
                        select q;
            }
            else
            {
                query = from q in ent.Activity
                        where q.Status == status && q.isApproved == approved
                        select q;
            }


            return query.Count();


        }

        #region Activity Schedule

        public void DeleteActivitySchedules(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from asched in ent.ActivitySchedule
                        where asched.ActivityID == activityID
                        select asched;

            var Slots = query.AsEnumerable();

            foreach (var slot in Slots)
            {
                DeleteActivitySchedule(slot.ID);
            }
        }

        private void DeleteActivitySchedule(int activityScheduleID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from asched in ent.ActivitySchedule
                        where asched.ID == activityScheduleID
                        select asched;

            ActivitySchedule schedule = query.FirstOrDefault();
            if (schedule != null)
            {
                ent.ActivitySchedule.Remove(schedule);
                ent.SaveChanges();
            }
        }

        #endregion

        #endregion

        #region Menu

        private int GetMaxMenuSequence(int? menuID, int? parentMenuItemID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from m in ent.Menu
                        where m.ID == menuID && m.ParentMenuID == parentMenuItemID
                        select m.Sequence;

            return Convert.ToInt32((query.Max()));
        }

        public void CreateMenu(DataSetComponent.MenuRow dr)
        {
            ISDEntities ent = new ISDEntities();
            Menu menu = new Menu();
            int linkID;
            CreateLink(dr, out linkID);

            menu.ParentMenuID = dr.ParentMenuID;
            menu.LinkID = linkID;
            menu.Sequence = GetMaxMenuSequence(menu.ID, menu.ParentMenuID) + 1;

            ent.Menu.Add(menu);
            ent.SaveChanges();

            dr.ID = menu.ID;
        }

        public void UpdateMenu(DataSetComponent.v_MenuRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from m in ent.Menu
                        where m.ID == dr.ID
                        select m;

            Menu menu = query.FirstOrDefault();

            if (menu != null)
            {
                //ObjectHandler.CopyProperties(dr, menu);
                UpdateLink(dr, Convert.ToInt32(menu.LinkID));
                menu.ParentMenuID = dr.ParentMenuID;
                //menu.Sequence = dr.Sequence;
                ent.SaveChanges();
            }
        }

        public DataSetComponent.MenuDataTable RetrieveChildMenuItems(int parentID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from m in ent.Menu
                        where m.ParentMenuID == parentID
                        select m;

            IEnumerable<Menu> menuItems = query.AsEnumerable();
            if (menuItems == null)
                return null;
            else
            {
                DataSetComponent.MenuDataTable dt = new DataSetComponent.MenuDataTable();
                ObjectHandler.CopyEnumerableToDataTable(menuItems, dt, LoadOption.PreserveChanges);
                return dt;
            }

        }

        public void CreateMenu(DataSetComponent.v_MenuRow dr)
        {
            ISDEntities ent = new ISDEntities();
            Menu menu = new Menu();
            int linkID;
            CreateLink(dr, out linkID);

            menu.ParentMenuID = dr.ParentMenuID;
            menu.MenuType = dr.MenuType;
            menu.LinkID = linkID;
            menu.Sequence = GetMaxMenuSequence(menu.ID, menu.ParentMenuID) + 1;

            ent.Menu.Add(menu);
            ent.SaveChanges();

            dr.ID = menu.ID;
        }

        public DataSetComponent.MenuDataTable RetrieveChildMenus(int parentID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from m in ent.Menu
                        where m.ParentMenuID == parentID
                        select m;

            IEnumerable<Menu> Menus = query.AsEnumerable();
            if (Menus == null)
                return null;
            else
            {
                DataSetComponent.MenuDataTable dt = new DataSetComponent.MenuDataTable();
                ObjectHandler.CopyEnumerableToDataTable(Menus, dt, LoadOption.PreserveChanges);
                return dt;
            }

        }

        public DataSetComponent.MenuRow RetrieveMenu(int SelectedMenuID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from m in ent.Menu
                        where m.ID == SelectedMenuID
                        select m;

            Menu menu = query.FirstOrDefault();
            DataSetComponent.MenuRow dr = new DataSetComponent.MenuDataTable().NewMenuRow();
            if (menu != null)
            {
                ObjectHandler.CopyPropertyValues(menu, dr);
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.v_MenuRow RetrieveMenuExplorer(int SelectedMenuID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from m in ent.v_Menu
                        where m.ID == SelectedMenuID
                        select m;

            v_Menu menu = query.FirstOrDefault();
            DataSetComponent.v_MenuRow dr = new DataSetComponent.v_MenuDataTable().Newv_MenuRow();
            if (menu != null)
            {
                ObjectHandler.CopyPropertyValues(menu, dr);
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.MenuDataTable RetrieveMenus()
        {
            ISDEntities ent = new ISDEntities();

            var query = from v in ent.Menu
                        orderby v.Sequence
                        select v;


            DataSetComponent.MenuDataTable dt = new DataSetComponent.MenuDataTable();
            var Menu = query.AsEnumerable();

            ObjectHandler.CopyEnumerableToDataTable(Menu, dt, LoadOption.PreserveChanges);
            return dt;
        }

        public void UpdateMenu(DataSetComponent.MenuRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from m in ent.Menu
                        where m.ID == dr.ID
                        select m;

            Menu menu = query.FirstOrDefault();

            if (menu != null)
            {
                //ObjectHandler.CopyProperties(dr, menu);
                UpdateLink(RetrieveMenuExplorer(dr.ID), Convert.ToInt32(menu.LinkID));
                menu.ParentMenuID = dr.ParentMenuID;
                menu.Sequence = dr.Sequence;
                ent.SaveChanges();
            }
        }

        public DataSetComponent.v_MenuDataTable RetrieveMenuExplorers(int menuType)
        {
            ISDEntities ent = new ISDEntities();

            var query = from v in ent.v_Menu
                        where v.MenuType == menuType
                        orderby v.Sequence
                        select v;


            DataSetComponent.v_MenuDataTable dt = new DataSetComponent.v_MenuDataTable();
            var Menu = query.AsEnumerable();

            ObjectHandler.CopyEnumerableToDataTable(Menu, dt, LoadOption.PreserveChanges);
            return dt;
        }

        public void DeleteMenu(int menuID, out int linkID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.Menu
                        where p.ID == menuID
                        select p;

            Menu menu = query.FirstOrDefault();

            if (menu != null)
            {
                linkID = Convert.ToInt32(menu.LinkID);
                ent.Menu.Add(menu);
                ent.SaveChanges();
            }
            linkID = 0;
        }

        public void DeleteLink(int linkID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from l in ent.Link
                        where l.ID == linkID
                        select l;

            Link obj = query.FirstOrDefault();

            if (obj != null)
            {
                ent.Link.Remove(obj);
                ent.SaveChanges();
            }
        }

        private void UpdateLink(DataSetComponent.v_MenuRow dr, int LinkID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from l in ent.Link
                        where l.ID == LinkID
                        select l;
            Link link = query.FirstOrDefault();

            link.LinkText = dr.LinkText;
            link.LinkType = dr.LinkType;
            link.LinkValue = dr.LinkValue;

            ent.SaveChanges();
        }

        public DataSetComponent.ProviderProfilesDataTable RetrieveAllproviders()
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.ProviderProfiles
                        select p;

            var provs = query.AsEnumerable();
            if (provs != null)
            {
                var dt = new DataSetComponent.ProviderProfilesDataTable();
                ObjectHandler.CopyEnumerableToDataTable(provs, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        private void CreateLink(DataSetComponent.MenuRow dr, out int linkID)
        {
            ISDEntities ent = new ISDEntities();
            Link link = new Link();

            ObjectHandler.CopyPropertyValues(dr, link);
            ent.Link.Add(link);
            ent.SaveChanges();
            linkID = link.ID;
        }

        private void CreateLink(DataSetComponent.v_MenuRow dr, out int linkID)
        {
            ISDEntities ent = new ISDEntities();

            Link link = new Link();

            link.LinkText = dr.LinkText;
            link.LinkType = dr.LinkType;
            link.LinkValue = dr.LinkValue;

            ent.Link.Add(link);
            ent.SaveChanges();
            linkID = link.ID;
        }


        #endregion

        #region Activity

        public DataSetComponent.v_ActivityExplorerRow RetrieveActivityExplorer(int ActivityID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;

            query = from a in ent.v_ActivityExplorer
                    where a.ID == ActivityID
                    select a;

            if (query.Count() == 0)
                return null;
            var activity = query.FirstOrDefault();

            if (activity != null)
            {
                DataSetComponent.v_ActivityExplorerRow dr = new DataSetComponent.v_ActivityExplorerDataTable().Newv_ActivityExplorerRow();
                ObjectHandler.CopyPropertyValues(activity, dr);
                if (string.IsNullOrEmpty(activity.Suburb))
                {
                    dr.Suburb = "";
                }
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.ActivityRow RetrieveActivity(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == activityID
                        select a;

            if (query.Count() == 0)
                return null;
            IEnumerable<Activity> activities = query.AsEnumerable();

            if (activities != null)
            {
                DataSetComponent.ActivityDataTable dt = new DataSetComponent.ActivityDataTable();
                DataSetComponent.ActivityRow dr = new DataSetComponent.ActivityDataTable().NewActivityRow();

                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.ActivityGroupingRow RetrieveActivityGroup(int ActivityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from g in ent.ActivityGrouping
                        where g.ActivityID == ActivityID
                        select g;

            if (query != null)
            {
                ActivityGrouping group = query.FirstOrDefault();
                var dr = new DataSetComponent.ActivityGroupingDataTable().NewActivityGroupingRow();
                ObjectHandler.CopyPropertyValues(group, dr);
                return dr;
            }
            else return null;
        }

        public void UpdateActivity(DataSetComponent.ActivityRow drDetail)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == drDetail.ID
                        select a;

            Activity act = query.FirstOrDefault();
            if (act != null)
            {
                ObjectHandler.CopyPropertyValues(drDetail, act);
                ent.SaveChanges();
            }
        }

        public void UpdateActivityContactDetail(DataSetComponent.ActivityContactDetailRow contactDetails)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.ActivityContactDetail
                        where ac.ActivityID == contactDetails.ActivityID
                        select ac;

            ActivityContactDetail act = query.FirstOrDefault();
            if (act != null)
            {
                contactDetails.ID = act.ID;
                ObjectHandler.CopyPropertyValues(contactDetails, act);
                ent.SaveChanges();
            }
        }

        public void UpdateActivityGrouping(DataSetComponent.ActivityGroupingRow drActGrouping)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.ActivityGrouping
                        where ac.ActivityID == drActGrouping.ActivityID
                        select ac;

            ActivityGrouping act = query.FirstOrDefault();
            if (act != null)
            {
                drActGrouping.ID = act.ID;
                ObjectHandler.CopyPropertyValues(drActGrouping, act);
                ent.SaveChanges();
            }
        }

        public void CreateActivitySchedule(DataSetComponent.ActivityScheduleRow ActScheduleDR)
        {
            ISDEntities ent = new ISDEntities();

            ActivitySchedule ActSched = new ActivitySchedule();
            ObjectHandler.CopyPropertyValues(ActScheduleDR, ActSched);

            ent.ActivitySchedule.Add(ActSched);
            ent.SaveChanges();

        }

        public int RetrieveActivitiesInCategoryCount(int CategoryID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from act in ent.Activity
                        where act.CategoryID == CategoryID || act.SecondaryCategoryID1 == CategoryID || act.SecondaryCategoryID2 == CategoryID ||
                        act.SecondaryCategoryID3 == CategoryID || act.SecondaryCategoryID4 == CategoryID
                        select act;
            return query.Count();
        }

        #region ActivitySchedule

        public DataSetComponent.ActivityScheduleDataTable RetrieveActivitySchedules(int ActivityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.ActivitySchedule
                        where a.ActivityID == ActivityID
                        select a;

            var actSched = query.AsEnumerable();

            if (actSched.Count() != 0)
            {
                var dt = new DataSetComponent.ActivityScheduleDataTable();
                actSched.CopyEnumerableToDataTable(dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public void UpdateActivitySchedule(DataSetComponent.ActivityScheduleRow drSched)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.ActivitySchedule
                        where a.ID == drSched.ID
                        select a;

            ActivitySchedule act = query.FirstOrDefault();
            if (act != null)
            {
                ObjectHandler.CopyPropertyValues(drSched, act);
                ent.SaveChanges();
            }
        }
        #endregion

        #endregion

        #region ActivityImage

        public void createActivityImageInformation(DataSetComponent.ActivityImageRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ActivityImage ii = new ActivityImage();
            ObjectHandler.CopyPropertyValues(dr, ii);

            ent.ActivityImage.Add(ii);
            ent.SaveChanges();
        }

        public void createActivityImageInformation(DataSetComponent.ActivityImageRow dr, out int iiID)
        {
            ISDEntities ent = new ISDEntities();
            ActivityImage ii = new ActivityImage();
            ObjectHandler.CopyPropertyValues(dr, ii);

            ent.ActivityImage.Add(ii);
            ent.SaveChanges();
            iiID = ii.ID;
        }

        public void UpdateImageInformation(int activityID, int iiID, DataSetComponent.ActivityImageRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ii in ent.ActivityImage
                        where ii.ActivityID == dr.ActivityID && ii.ID == dr.ID
                        select ii;

            ActivityImage actImage = query.FirstOrDefault();
            if (actImage != null)
                ObjectHandler.CopyPropertyValues(dr, actImage);

            ent.SaveChanges();
        }

        public void CreateActivityImage(DataSetComponent.ActivityImageDetailRow dr, out int imageID1)
        {
            ISDEntities ent = new ISDEntities();
            ActivityImageDetail ai = new ActivityImageDetail();
            ObjectHandler.CopyPropertyValues(dr, ai);
            ent.ActivityImageDetail.Add(ai);
            ent.SaveChanges();
            imageID1 = ai.ID;
        }

        public void CreateActivityImage(DataSetComponent.ActivityImageDetailRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ActivityImageDetail ai = new ActivityImageDetail();
            ObjectHandler.CopyPropertyValues(dr, ai);
            ent.ActivityImageDetail.Add(ai);
            ent.SaveChanges();
        }

        public void UpdateActivityImage(DataSetComponent.ActivityImageDetailRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.ActivityImageDetail
                        where p.ActivityID == dr.ActivityID && p.ID == dr.ID
                        select p;

            ActivityImageDetail actImage = query.FirstOrDefault();
            dr.ImageStream = actImage.ImageStream;
            if (actImage != null)
                ObjectHandler.CopyPropertyValues(dr, actImage);

            ent.SaveChanges();
        }

        public void DeleteActivityImage(int activityID, int imageID, out string imageThumbVirtualPath, out string imageVirtualPath)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.ActivityImageDetail
                        where p.ActivityID == activityID && p.ID == imageID
                        select p;

            ActivityImageDetail prodImage = query.FirstOrDefault();
            imageVirtualPath = SystemConstants.ActImageDirectory + "/" + activityID + "/" + activityID + "_" + imageID + "_" + prodImage.Filename;
            imageThumbVirtualPath = SystemConstants.ActImageDirectory + "/" + activityID + "/" + SystemConstants.ImageThumbDirectory + activityID + "_" + imageID + "_" + prodImage.Filename;
            if (prodImage != null)
                ent.ActivityImageDetail.Remove(prodImage);

            ent.SaveChanges();
        }

        public DataSetComponent.ActivityImageRow RetrieveActivityImageInformation(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImage
                        where i.ActivityID == activityID
                        select i;
            var ii = query.FirstOrDefault();
            if (ii != null)
            {
                DataSetComponent.ActivityImageRow dr = new DataSetComponent.ActivityImageDataTable().NewActivityImageRow();
                ObjectHandler.CopyPropertyValues(ii, dr);
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.ActivityImageDetailDataTable RetrieveActivityImages(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID
                        orderby i.ID
                        select i;

            DataSetComponent.ActivityImageDetailDataTable dt = new DataSetComponent.ActivityImageDetailDataTable();
            ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);

            return dt;
        }

        public int RetrieveActivityImagesCount(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID
                        orderby i.ID
                        select i;

            return query.Count();
        }

        public DataSetComponent.ActivityImageDetailRow RetrievePrimaryProductImage(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID && i.isPrimaryImage == true
                        select i;

            DataSetComponent.ActivityImageDetailRow dr = new DataSetComponent.ActivityImageDetailDataTable().NewActivityImageDetailRow();
            if (query.FirstOrDefault() != null)
            {
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
            }
            return dr;
        }

        public DataSetComponent.ActivityImageDetailRow RetrieveProductMainImage(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID && i.isPrimaryImage == true
                        select i;

            DataSetComponent.ActivityImageDetailRow dr = new DataSetComponent.ActivityImageDetailDataTable().NewActivityImageDetailRow();
            ActivityImageDetail pi = query.FirstOrDefault();
            if (pi != null)
                ObjectHandler.CopyPropertyValues(pi, dr);
            else
            {
                dr.ActivityID = 0;
                dr.ID = 0;
            }
            return dr;
        }

        public DataSetComponent.v_ActivityImageExplorerRow RetrieveActivityImage(int activityID, int imageID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.v_ActivityImageExplorer
                        where i.ActivityID == activityID && i.ImageID == imageID
                        select i;

            DataSetComponent.v_ActivityImageExplorerRow dr = new DataSetComponent.v_ActivityImageExplorerDataTable().Newv_ActivityImageExplorerRow();
            v_ActivityImageExplorer ai = query.FirstOrDefault();
            if (ai != null)
                ObjectHandler.CopyPropertyValues(ai, dr);

            return dr;
        }

        public void UpdateActivityPrimaryImage(int activityID, int imageID)
        {
            ISDEntities ent = new ISDEntities();
            var setMainFalse = from fi in ent.ActivityImageDetail
                               where fi.isPrimaryImage == true && fi.ActivityID == activityID
                               select fi;

            ActivityImageDetail pif = setMainFalse.FirstOrDefault();
            if (pif != null)
            {
                pif.isPrimaryImage = false;
                ent.SaveChanges();
            }

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID && i.ID == imageID
                        select i;

            ActivityImageDetail pit = query.FirstOrDefault();
            if (pit != null)
            {
                pit.isPrimaryImage = true;
                ent.SaveChanges();
            }
        }
        #endregion

        #region log

        public void createActivityLogGroup(DataSetComponent.ActivitiesLogGroupRow actLogGroup, out int ActivityLogGroupID)
        {
            ISDEntities ent = new ISDEntities();
            ActivitiesLogGroup actloggr = new ActivitiesLogGroup();
            ObjectHandler.CopyPropertyValues(actLogGroup, actloggr);
            ent.ActivitiesLogGroup.Add(actloggr);
            ent.SaveChanges();
            ActivityLogGroupID = actloggr.ID;
        }

        public DataSetComponent.ActivitiesLogGroupRow RetrievePastActivityLogGroup(int activityID, int LastNotificationType, DateTime ExpiryDate)
        {
            ISDEntities ent = new ISDEntities();
            var query = from alg in ent.ActivitiesLogGroup
                        where alg.ActivityID == activityID && alg.ExpiryDate == ExpiryDate
                        && alg.LastNotificationType == LastNotificationType
                        select alg;

            var actlg = query.FirstOrDefault();
            if (actlg != null)
            {
                var dr = new DataSetComponent.ActivitiesLogGroupDataTable().NewActivitiesLogGroupRow();
                ObjectHandler.CopyPropertyValues(actlg, dr);
                return dr;
            }
            else return null;
        }

        public void UpdateActivityLogGroup(int activityLogGroupID, DataSetComponent.ActivitiesLogGroupRow actLogGroup)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.ActivitiesLogGroup
                        where c.ID == activityLogGroupID
                        select c;

            ActivitiesLogGroup web = query.FirstOrDefault();

            if (web != null)
            {
                ObjectHandler.CopyPropertyValues(actLogGroup, web);
                ent.SaveChanges();
            }
        }

        public void UpdateActivityogGroup(DataSetComponent.ActivitiesLogGroupRow actLogGroup, int actLogGroupID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.ActivitiesLogGroup
                        where c.ID == actLogGroupID
                        select c;

            ActivitiesLogGroup web = query.FirstOrDefault();

            if (web != null)
            {
                ObjectHandler.CopyPropertyValues(actLogGroup, web);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.ActivitiesLogGroupDataTable RetrieveActivityLogGroups()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.ActivitiesLogGroup

                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.ActivitiesLogGroupDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);

                return dt;
            }
            else return null;
        }

        public DataSetComponent.ActivitiesLogDataTable RetrieveActivitiesLogActions(int ActivitiesLogGroupID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.ActivitiesLog
                        where q.ActivityLogGroupID == ActivitiesLogGroupID
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.ActivitiesLogDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);

                return dt;
            }
            else return null;
        }

        public void UpdateActivityLogNote(int activityLogID, string noteMessage)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.ActivitiesLog
                        where q.ID == activityLogID
                        select q;

            var act = query.FirstOrDefault();
            if (act != null)
            {
                act.Note = noteMessage;
                ent.SaveChanges();
            }
        }

        public string RetrieveActivityLogNote(int activityLogID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.ActivitiesLog
                        where q.ID == activityLogID
                        select q;

            var act = query.FirstOrDefault();
            if (act != null)
            {
                return act.Note;
            }
            else return null;
        }

        public DataSetComponent.ActivitiesLogGroupRow RetrieveActivitiesLogGroup(int activityID, DateTime expiryDate)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.ActivitiesLogGroup
                        where q.ActivityID == activityID && q.ExpiryDate == expiryDate.Date
                        select q;

            var act = query.FirstOrDefault();
            if (act != null)
            {
                var dr = new DataSetComponent.ActivitiesLogGroupDataTable().NewActivitiesLogGroupRow();
                BCUtility.ObjectHandler.CopyPropertyValues(act, dr);
                return dr;
            }
            else return null;
        }

        public void SaveWebLogAction(DataSetComponent.WeblLogActionRow dr, out int LogActionID)
        {
            ISDEntities ent = new ISDEntities();
            WeblLogAction logAct = new WeblLogAction();

            ObjectHandler.CopyPropertyValues(dr, logAct);
            ent.WeblLogAction.Add(logAct);
            ent.SaveChanges();
            LogActionID = logAct.ID;
        }

        public void ChangeStatus(int actID, int activityStatus)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == actID
                        select a;
            Activity act = query.FirstOrDefault();

            if (act != null)
            {
                act.Status = activityStatus;
                ent.SaveChanges();
            }
        }

        public void ChangeStatus(List<StatusChange> stats)
        {
            ISDEntities ent = new ISDEntities();
            IEnumerable<int> actIDs = stats.Select(x => x.ActID);

            List<Activity> acts = (from a in ent.Activity
                                   where actIDs.Contains(a.ID)
                                   select a).ToList();
            foreach (var act in acts)
            {
                if (act != null)
                {
                    int StatusChangeTo = stats.Where(x => x.ActID == act.ID).Select(x => x.StatusChangeTo).FirstOrDefault();
                    act.Status = StatusChangeTo;
                }
            }

            ent.SaveChanges();
        }

        public void SaveLog(DataSetComponent.WebLogRow drLog, out int WebLogID)
        {
            ISDEntities ent = new ISDEntities();
            WebLog log = new WebLog();

            ObjectHandler.CopyPropertyValues(drLog, log);
            ent.WebLog.Add(log);
            ent.SaveChanges();
            WebLogID = log.ID;
        }

        public DataSetComponent.WebLogDataTable RetrieveLogs(int category)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.WebLog
                        where q.LogCategory == category
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.WebLogDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);

                return dt;
            }
            else return null;
        }

        public DataSetComponent.WeblLogActionDataTable RetrieveLogActions(int webLogID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.WeblLogAction
                        where q.WebLogID == webLogID
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.WeblLogActionDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);


                return dt;
            }
            else return null;
        }

        public void SaveActivityLog(DataSetComponent.ActivitiesLogRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ActivitiesLog actLog = new ActivitiesLog();

            ObjectHandler.CopyPropertyValues(dr, actLog);
            ent.ActivitiesLog.Add(actLog);
            ent.SaveChanges();
        }

        #endregion


        public void UpdateWebConfigurationColor(DataSetComponent.WebConfigurationRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.WebConfiguration
                        select c;

            WebConfiguration web = query.FirstOrDefault();

            if (web != null)
            {
                ObjectHandler.CopyPropertyValues(dr, web);
                ent.SaveChanges();
            }
        }

        #region Emailer

        public DataSetComponent.WebConfigurationRow RetrieveEmailer()
        {
            ISDEntities ent = new ISDEntities();

            var query = from w in ent.WebConfiguration
                        select w;

            WebConfiguration web = query.FirstOrDefault();
            if (web != null)
            {
                DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
                ObjectHandler.CopyPropertyValues(web, dr);
                if (dr.IsSMTPAccountNull())
                    return null;
                else return dr;
            }
            else return null;
        }

        public void EditEmailer(DataSetComponent.WebConfigurationRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from w in ent.WebConfiguration
                        select w;

            WebConfiguration web = query.FirstOrDefault();
            if (web != null)
            {
                web.SMTPAccount = dr.SMTPAccount;
                web.SMTPHost = dr.SMTPHost;
                web.SMTPUserName = dr.SMTPUserName;
                web.SMTPPassword = dr.SMTPPassword;
                web.SMTPPort = dr.SMTPPort;
                web.SMTPSSL = dr.SMTPSSL;
                web.SMTPIIS = dr.SMTPIIS;
                ent.SaveChanges();
            }
            else if (web == null && web.SMTPAccount == null)
            {
                CreateEmailer(dr);
            }

        }

        public void CreateEmailer(DataSetComponent.WebConfigurationRow dr)
        {
            ISDEntities ent = new ISDEntities();
            WebConfiguration web = new WebConfiguration();

            web.SMTPAccount = dr.SMTPAccount;
            web.SMTPHost = dr.SMTPHost;
            web.SMTPUserName = dr.SMTPUserName;
            web.SMTPPassword = dr.SMTPPassword;
            web.SMTPPort = dr.SMTPPort;
            web.SMTPSSL = dr.SMTPSSL;

            ent.WebConfiguration.Add(web);
            ent.SaveChanges();
        }

        public void SaveEmailSettings(DataSetComponent.EmailSettingDataTable dt)
        {
            ClearEmailSettings();
            ISDEntities ent = new ISDEntities();
            foreach (var dr in dt)
            {
                EmailSetting email = new EmailSetting();
                ObjectHandler.CopyPropertyValues(dr, email);
                ent.EmailSetting.Add(email);
            }

            ent.SaveChanges();

        }

        public void ClearEmailSettings()
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.EmailSetting
                        select e;

            var settings = query.AsEnumerable();
            ent.EmailSetting.RemoveRange(settings);

            ent.SaveChanges();
        }

        public DataSetComponent.EmailSettingDataTable RetrieveEmailSettings()
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.EmailSetting
                        select e;

            var settings = query.AsEnumerable();
            if (settings != null)
            {
                var dt = new DataSetComponent.EmailSettingDataTable();
                ObjectHandler.CopyEnumerableToDataTable(settings, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;

        }

        #endregion



        #region asset

        public void DeleteAsset(int assetID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from w in ent.WebAssets
                        where w.ID == assetID
                        select w;

            WebAssets cat = query.FirstOrDefault();
            if (cat != null)
            {
                ent.WebAssets.Remove(cat);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.WebAssetsDataTable RetrieveWebAssets(int startIndex, int amount)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.WebAssets
                        select e;

            var assets = query.AsEnumerable();
            if (assets != null)
            {
                var dt = new DataSetComponent.WebAssetsDataTable();
                ObjectHandler.CopyEnumerableToDataTable(assets, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int RetrieveWebAssetsCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.WebAssets
                        select e;

            return query.Count();
        }

        public void CreateAssetInformation(DataSetComponent.WebAssetsRow dr)
        {
            ISDEntities ent = new ISDEntities();
            WebAssets cat = new WebAssets();

            ObjectHandler.CopyPropertyValues(dr, cat);

            ent.WebAssets.Add(cat);
            ent.SaveChanges();
        }

        #endregion

        #region emailtemplate



        #endregion

        /*
        public DataSetComponent.ActivitiesLogDataTable RetrieveActivityLog(int ActivityID, int NotificationNumber, DateTime ExpiryDate)
        {
            ISDEntities ent = new ISDEntities();
            var expiry = ExpiryDate.ToShortDateString();

            var query = from al in ent.v_ActivitiesLogExplorer
                        where al.ActivityID == ActivityID && al.Value == expiry
                        select al;

            var actLog = query.AsEnumerable();
            if (actLog.Count() != 0)
            {
                var dt = new DataSetComponent.ActivitiesLogDataTable();
                BCUtility.ObjectHandler.CopyEnumerableToDataTable(actLog, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }
        */

        #region Activity

        public void ConfirmActivity(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == activityID
                        select a;

            Activity act = query.FirstOrDefault();
            if (act != null)
            {
                act.isApproved = true;
                act.ModifiedBy = "Admin";
                act.ModifiedDateTime = DateTime.Now;
                ent.SaveChanges();
            }
        }

        public void DeleteActivity(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == activityID
                        select a;

            Activity act = query.FirstOrDefault();
            if (act != null)
            {
                ent.Activity.Remove(act);
                ent.SaveChanges();
            }
        }


        public void UpdateActivities(DataSetComponent.ActivityRow drDetail)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == drDetail.ID
                        select a;

            Activity act = query.FirstOrDefault();
            if (act != null)
            {
                drDetail.ID = act.ID;
                ObjectHandler.CopyPropertyValues(drDetail, act);
                ent.SaveChanges();
            }
        }

        public void ChangeStatus(int actID, bool isActive)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == actID
                        select a;
            Activity act = query.FirstOrDefault();

            if (act != null)
            {
                if (isActive)
                    act.Status = (int)SystemConstants.ActivityStatus.Active;
                else
                    act.Status = (int)SystemConstants.ActivityStatus.NotActive;

                ent.SaveChanges();
            }
        }

        public void ChangeStatus(int actID, int Status, string Username)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.Activity
                        where a.ID == actID
                        select a;
            Activity act = query.FirstOrDefault();

            if (act != null)
            {
                act.Status = Status;
                act.ModifiedBy = Username;
                act.ModifiedDateTime = DateTime.Now;
                ent.SaveChanges();
            }
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesFilteredReport(String ProviderID, string SearchKey, int ageFrom, int ageTo, int postCode, int CategoryID)
        {
            String[] Keywords = Array.ConvertAll(SearchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IEnumerable<v_ActivityView> activitiesReport;

            IQueryable<v_ActivityView> providerCatFiltered;
            IQueryable<v_ActivityView> paramFiltered;
            if (CategoryID == 0)
                providerCatFiltered = from a in ent.v_ActivityView
                                      where a.ProviderID == ProviderID
                                      orderby a.ID
                                      select a;
            else
                providerCatFiltered = from a in ent.v_ActivityView
                                      where (a.ProviderID == ProviderID && (
                                                      a.CategoryID == CategoryID ||
                                                      a.CategoryLevel1ParentID == CategoryID ||
                                                      a.CategoryLevel2ParentID == CategoryID))
                                      select a;


            paramFiltered = from a in providerCatFiltered
                            where a.AgeFrom <= ageFrom && a.AgeTo >= ageTo
                            select a;

            if (postCode != 0)
                paramFiltered = from a in providerCatFiltered
                                where a.SuburbID == postCode
                                select a;


            if (!string.IsNullOrEmpty(SearchKey))
            {
                var querySearchFiltered = (from a in paramFiltered
                                           from w in Keywords
                                           where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                                           || a.Keywords.ToLower().Contains(w.ToLower())
                                           select a).Distinct().ToArray();
                activitiesReport = querySearchFiltered.AsEnumerable();
            }
            else
            {
                activitiesReport = paramFiltered.AsEnumerable();
            }

            var dt = new DataSetComponent.v_ActivityViewDataTable();
            if (activitiesReport != null)
            {
                ObjectHandler.CopyEnumerableToDataTable(activitiesReport, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;
        }

        public DataSetComponent.ActivityScheduleDataTable RetrieveActivitiesSchedulesbyIDs(HashSet<int> activityID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<ActivitySchedule> query = null;

            query = from a in ent.ActivitySchedule
                    select a;
            query = query.Where(a => activityID.Contains(a.ActivityID));


            if (query.Count() == 0)
                return null;
            IEnumerable<ActivitySchedule> actsched = query.AsEnumerable();

            if (actsched != null)
            {
                DataSetComponent.ActivityScheduleDataTable dt = new DataSetComponent.ActivityScheduleDataTable();
                ObjectHandler.CopyEnumerableToDataTable(actsched, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;
        }

        private HashSet<int> FilterActivitiesDay(DataSetComponent.ActivityScheduleDataTable dt, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            HashSet<int> actIDs = new HashSet<int>();

            var scheds = from s in dt
                         where (s.OnMonday == mon && mon != false) || (s.OnTuesday == tue && tue != false) || (s.OnWednesday == wed && wed != false)
                         || (s.OnThursday == thu && thu != false) || (s.OnFriday == fri && fri != false) || (s.OnSaturday == sat && sat != false) || (s.OnSunday == sun && sun != false)
                         select s.ActivityID;
            actIDs = new HashSet<int>(scheds.AsEnumerable());
            return actIDs;
        }

        private HashSet<int> FilterActivitiesDate(DataSetComponent.ActivityScheduleDataTable dt, DateTime dtFrom, DateTime dtTo)
        {

            //DataSetComponent.ActivityScheduleDataTable dt = RetrieveActivitiesSchedulesbyIDs(actIDs);

            var query = from a in dt
                        where (dtFrom.DayOfYear >= a.ActivityStartDatetime.DayOfYear || dtTo.DayOfYear <= a.ActivityExpiryDate.DayOfYear)
                        select a;

            if (dtFrom.TimeOfDay != SystemConstants.nodate.TimeOfDay || dtTo.TimeOfDay != SystemConstants.nodate.TimeOfDay)
            {
                query = from a in query
                        where (dtFrom.TimeOfDay <= a.ActivityStartDatetime.TimeOfDay && dtTo.TimeOfDay >= a.ActivityEndDatetime.TimeOfDay)
                        select a;
            }
            if (query.AsEnumerable() != null)
            {
                HashSet<int> actIDs = new HashSet<int>(query.AsEnumerable().Select(x => x.ActivityID));
                return actIDs;
            }
            else return null;
        }

        private HashSet<int> FilterActivitiesTime(DataSetComponent.ActivityScheduleDataTable dt, TimeSpan tmFrom, TimeSpan tmTo)
        {

            //DataSetComponent.ActivityScheduleDataTable dt = RetrieveActivitiesSchedulesbyIDs(actIDs);

            if (tmFrom != SystemConstants.nodate.TimeOfDay || tmTo != SystemConstants.nodate.TimeOfDay)
            {
                var query = from a in dt
                            where (tmFrom <= a.ActivityStartDatetime.TimeOfDay && tmFrom <= a.ActivityEndDatetime.TimeOfDay) || (tmTo <= a.ActivityStartDatetime.TimeOfDay && tmTo <= a.ActivityEndDatetime.TimeOfDay)
                            select a;
                if (query.AsEnumerable() != null)
                {
                    HashSet<int> actIDs = new HashSet<int>(query.AsEnumerable().Select(x => x.ActivityID));
                    return actIDs;
                }
                else return null;
            }
            else return null;

        }

        public DataSetComponent.UserImageDetailRow RetrieveProviderPrimaryImage(String providerID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from pi in ent.UserImageDetail
                        where pi.UserID == providerID && pi.isPrimaryImage == true
                        select pi;

            UserImageDetail detail = query.FirstOrDefault();
            if (detail != null)
            {
                var dr = new DataSetComponent().UserImageDetail.NewUserImageDetailRow();
                ObjectHandler.CopyPropertyValues(detail, dr);
                return dr;
            }
            else return null;
        }

        #endregion

        public DataSetComponent.ActivityDataTable RetrieveActivitiesbyIDs(List<int> selectedDT)
        {
            ISDEntities ent = new ISDEntities();

            HashSet<int> selectedActivityID = new HashSet<int>(selectedDT.Select(x => x));
            var query = ent.Activity.Where(x => selectedActivityID.Contains(x.ID));

            if (query.Count() != 0)
            {
                var dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }


        public string RetrieveActivityNamebyID(int actid)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.Activity
                        where e.ID == actid
                        select e;
            var act = query.FirstOrDefault();
            if (act != null)
            {

                return act.Name;
            }
            else return null;
        }


        public DataSetComponent.ActivityScheduleDataTable RetrieveActivitySchedulesbyIDs(List<int> selectedDT)
        {
            ISDEntities ent = new ISDEntities();

            HashSet<int> selectedActivityID = new HashSet<int>(selectedDT.Select(x => x));
            var query = ent.ActivitySchedule.Where(x => selectedActivityID.Contains(x.ActivityID));

            if (query.Count() != 0)
            {
                var dt = new DataSetComponent.ActivityScheduleDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }



        public List<int> RetrieveExpiredActivityIDs()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<Activity> query = ent.Activity.Where(x => x.Status.Equals((int)SystemConstants.ActivityStatus.Expired));

            if (query.AsEnumerable() != null && query.Count() > 0)
            {
                return new List<int>(query.Select(x => x.ID));
            }
            else return null;
        }

        public DataSetComponent.ActivityDataTable RetrieveExpiredActivities()
        {
            ISDEntities ent = new ISDEntities();

            IQueryable<Activity> query = ent.Activity.Where(x => x.Status.Equals((int)SystemConstants.ActivityStatus.Expired));

            if (query.AsEnumerable() != null && query.Count() > 0)
            {
                DataSetComponent.ActivityDataTable dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        #region Rewards

        public string getSponsorName(String spnID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from a in ent.Sponsor
                        where a.ID == spnID
                        select a;
            var act = query.FirstOrDefault();
            if (act != null)
            {
                return act.Name;
            }
            else return null;

        }

        public void DeleteSponsor(String sponsorid)
        {
            ISDEntities ent = new ISDEntities();

            var query2 = from a in ent.Sponsor
                         where a.ID == sponsorid
                         select a;

            Sponsor rwd2 = query2.FirstOrDefault();
            if (rwd2 != null)
            {
                ent.Sponsor.Remove(rwd2);
                ent.SaveChanges();
            }

        }

        public DataSetComponent.SponsorRow RetrieveSponsorDetails(String SponsorID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from a in ent.Sponsor
                        where a.ID == SponsorID
                        select a;
            if (query.Count() == 0)
            {
                return null;
            }
            var Sponsor = query.FirstOrDefault();
            if (Sponsor != null)
            {
                DataSetComponent.SponsorRow dr = new DataSetComponent.SponsorDataTable().NewSponsorRow();
                ObjectHandler.CopyPropertyValues(Sponsor, dr);
                return dr;
            }
            else

                return null;



        }

        public DataSetComponent.SponsorDataTable RetrieveSponsorsExplorer()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Sponsor
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.SponsorDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }



        public DataSetComponent.v_RewardExplorerDataTable RetrieveRewardsExplorer()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        select q;

            var act = query.AsEnumerable();
            if (act != null)
            {
                var dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(act, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int RetrieveRewardsExplorerCount()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        select q;

            return query.Count();


        }

        public void SaveReward(DataSetComponent.RewardRow drReward, out int RewardID)
        {
            ISDEntities ent = new ISDEntities();
            Reward rew = new Reward();
            ObjectHandler.CopyPropertyValues(drReward, rew);
            ent.Reward.Add(rew);
            ent.SaveChanges();
            RewardID = rew.ID;
        }

        public void UpdateSponsor(DataSetComponent.SponsorRow sr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.Sponsor
                        where ac.ID == sr.ID
                        select ac;

            Sponsor user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(sr, user);
                ent.SaveChanges();
            }

        }

        public void UpdateReward(DataSetComponent.RewardRow drReward)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.Reward
                        where ac.ID == drReward.ID
                        select ac;

            Reward user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(drReward, user);
                ent.SaveChanges();
            }
        }

        public int getDetailsID(int RewardID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.RewardsDetails
                        where ac.RewardID == RewardID
                        select ac;
            var act = query.FirstOrDefault();
            if (act != null)
            {
                return act.ID;
            }
            else return 0;

        }

        public int getImageID(int RewardID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.RewardImage
                        where ac.RewardID == RewardID
                        select ac;
            var act = query.FirstOrDefault();
            if (act != null)
            {
                return act.ID;
            }
            else return 0;

        }
        public void SaveSponsorDetail(DataSetComponent.SponsorRow drSpnDet)
        {
            ISDEntities ent = new ISDEntities();
            Sponsor det = new Sponsor();
            ObjectHandler.CopyPropertyValues(drSpnDet, det);
            ent.Sponsor.Add(det);
            ent.SaveChanges();
        }


        public void SaveRewardImage(DataSetComponent.RewardImageRow drRwrdImage)
        {
            ISDEntities ent = new ISDEntities();
            RewardImage img = new RewardImage();
            ObjectHandler.CopyPropertyValues(drRwrdImage, img);
            ent.RewardImage.Add(img);
            ent.SaveChanges();

        }

        public void UpdateRewardImage(DataSetComponent.RewardImageRow drReward)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.RewardImage
                        where ac.RewardID == drReward.RewardID
                        select ac;

            RewardImage user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(drReward, user);
                ent.SaveChanges();
            }
        }

        public void SaveRewardDetail(DataSetComponent.RewardsDetailsRow drRwrdDet)
        {
            ISDEntities ent = new ISDEntities();
            RewardsDetails det = new RewardsDetails();
            ObjectHandler.CopyPropertyValues(drRwrdDet, det);
            ent.RewardsDetails.Add(det);
            ent.SaveChanges();
        }

        public void UpdateRewardDetail(DataSetComponent.RewardsDetailsRow drReward)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.RewardsDetails
                        where ac.RewardID == drReward.RewardID
                        select ac;

            RewardsDetails user = query.FirstOrDefault();
            if (user != null)
            {
                ObjectHandler.CopyPropertyValues(drReward, user);
                ent.SaveChanges();
            }
        }

        public void DeleteReward(int RewardID)
        {
            ISDEntities ent = new ISDEntities();
            var query2 = from a in ent.RewardImage
                         where a.RewardID == RewardID
                         select a;

            RewardImage rwd2 = query2.FirstOrDefault();
            if (rwd2 != null)
            {
                ent.RewardImage.Remove(rwd2);
                ent.SaveChanges();
            }

            var query1 = from a in ent.RewardsDetails
                         where a.RewardID == RewardID
                         select a;

            RewardsDetails rwd1 = query1.FirstOrDefault();
            if (rwd1 != null)
            {
                ent.RewardsDetails.Remove(rwd1);
                ent.SaveChanges();
            }


            var query = from a in ent.Reward
                        where a.ID == RewardID
                        select a;

            Reward rwd = query.FirstOrDefault();
            if (rwd != null)
            {
                ent.Reward.Remove(rwd);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.v_RewardExplorerRow RetrieveRewardInfo(int RewardID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query = null;
            query = from i in ent.v_RewardExplorer
                    where i.ID == RewardID
                    select i;
            if (query.Count() == 0)
            {
                return null;
            }
            var Reward = query.FirstOrDefault();
            if (Reward != null)
            {
                DataSetComponent.v_RewardExplorerRow dr = new DataSetComponent.v_RewardExplorerDataTable().Newv_RewardExplorerRow();
                ObjectHandler.CopyPropertyValues(Reward, dr);
                return dr;
            }
            else

                return null;


        }

        public DataSetComponent.RewardsTypeDataTable RetrieveRewardTypes()
        {
            ISDEntities ent = new ISDEntities();

            var query = from rt in ent.RewardsType
                        select rt;

            if (query.AsEnumerable() != null)
            {
                var dt = new DataSetComponent.RewardsTypeDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;

        }

        public DataSetComponent.RewardsTypeRow RetrieveRewardType(int Rewardtype)
        {
            ISDEntities ent = new ISDEntities();

            var query = from rt in ent.RewardsType
                        where rt.Type == Rewardtype
                        select rt;

            if (query.FirstOrDefault() != null)
            {
                var dr = new DataSetComponent.RewardsTypeDataTable().NewRewardsTypeRow();
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
                return dr;
            }
            else return null;

        }

        public DataSetComponent.RewardImageRow RetrieveRewardPrimaryImage(int RewardID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.RewardImage
                        where i.RewardID == RewardID
                        select i;

            DataSetComponent.RewardImageRow dr = new DataSetComponent.RewardImageDataTable().NewRewardImageRow();
            RewardImage pi = query.FirstOrDefault();
            if (pi != null)
                ObjectHandler.CopyPropertyValues(pi, dr);
            else
            {
                //dr.ProductID = 0;
                //dr.ImageID = 0;
                return null;
            }
            return dr;
        }

        public byte[] RetrieveRewardBinary(int imgID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.RewardImage
                        where e.ID == imgID
                        select e;
            RewardImage aid = query.FirstOrDefault();

            if (aid != null && aid.ImageStream != null)
            {
                byte[] stream = aid.ImageStream;
                return stream;
            }
            else return null;
        }

        public int RetrieveActiveRewards()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.UsageTimes > q.NofTimeUsed
                        select q;

            return query.Count();


        }

        public int RetrieveInactiveRewards()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.UsageTimes <= q.NofTimeUsed
                        select q;

            return query.Count();


        }

        public int RetrieveExpiredRewards()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.RewardExpiryDate < DateTime.Now
                        select q;

            return query.Count();


        }

        public int RetrieveGifts()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.RewardType == (int)SystemConstants.RewardType.Gift
                        select q;

            return query.Count();


        }


        public int RetrieveOffers()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.RewardType == (int)SystemConstants.RewardType.Offer
                        select q;

            return query.Count();


        }


        public int RetrieveDiscounts()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.RewardType == (int)SystemConstants.RewardType.Discount
                        select q;

            return query.Count();


        }


        public int RetrieveOthers()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_RewardExplorer
                        where q.RewardType == (int)SystemConstants.RewardType.Other
                        select q;

            return query.Count();


        }

        public int Retrieveneverred()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.Reward
                        where q.NofTimeUsed == 0
                        select q;
            return query.Count();


        }

        public int RetrieveTotalRedempted()
        {
            ISDEntities ent = new ISDEntities();

            var query = from q in ent.v_VoucherExplorer
                        select q;
            return query.Count();


        }
        #endregion

        #region Council

        public void CreateCouncil(string userName, DataSetComponent.CouncilRow dr)
        {
            dr.CreatedBy = dr.ModifiedBy = userName;
            dr.CreatedDatetime = dr.ModifiedDatetime = DateTime.Now;

            ISDEntities ent = new ISDEntities();
            Council Council = new Council();
            ObjectHandler.CopyPropertyValues(dr, Council);
            ent.Council.Add(Council);
            ent.SaveChanges();
        }

        public void UpdateCouncil(string userName, DataSetComponent.CouncilRow dr)
        {
            dr.ModifiedDatetime = DateTime.Now;
            dr.ModifiedBy = userName;

            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Council
                        where c.ID == dr.ID
                        select c;

            Council council = query.FirstOrDefault();

            if (council != null)
            {
                ObjectHandler.CopyPropertyValues(dr, council);
                ent.SaveChanges();
            }


        }

        public void DeleteCouncil(int CouncilID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.Council
                        where s.ID == CouncilID
                        select s;

            Council council = query.FirstOrDefault();
            if (council != null)
            {
                ent.Council.Remove(council);
                ent.SaveChanges();
            }
        }

        public DataSetComponent.CouncilRow RetrieveCouncil(int councilID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Council
                        where c.ID == councilID
                        select c;

            var subs = query.FirstOrDefault();
            if (subs != null)
            {
                var dr = new DataSetComponent.CouncilDataTable().NewCouncilRow();
                ObjectHandler.CopyPropertyValues(subs, dr);
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.CouncilDataTable RetrieveCouncils()
        {
            ISDEntities ent = new ISDEntities();

            var query = from sub in ent.Council
                        select sub;

            var subs = query.AsEnumerable();
            if (subs != null)
            {
                var dt = new DataSetComponent.CouncilDataTable();
                ObjectHandler.CopyEnumerableToDataTable(subs, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;


        }

        public DataSetComponent.CouncilDataTable RetrieveCouncils(int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.Council
                        select s;

            DataSetComponent.CouncilDataTable dt = new DataSetComponent.CouncilDataTable();

            if (query.AsEnumerable() == null)
                return null;
            else
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);

                //ObjectHandler.CopyProperties(category, dr);
                return dt;
            }
        }

        public int RetrieveCouncilsCount(string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from s in ent.Council
                        select s;

            return query.Count();
        }

        public DataSetComponent.v_SuburbExplorerRow RetrieveSuburbCouncil(int suburbID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from coun in ent.v_SuburbExplorer
                        where coun.ID == suburbID
                        select coun;

            var subs = query.FirstOrDefault();
            if (subs != null)
            {
                var dr = new DataSetComponent.v_SuburbExplorerDataTable().Newv_SuburbExplorerRow();
                ObjectHandler.CopyPropertyValues(subs, dr);
                return dr;
            }
            else
                return null;


        }

        public DataSetComponent.v_CouncilExplorerRow RetrieveCouncilState(int councilID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from sub in ent.v_CouncilExplorer
                        where sub.ID == councilID
                        select sub;

            var subs = query.FirstOrDefault();
            if (subs != null)
            {
                var dt = new DataSetComponent.v_CouncilExplorerDataTable().Newv_CouncilExplorerRow();
                ObjectHandler.CopyPropertyValues(subs, dt);
                return dt;
            }
            else
                return null;


        }

        public int RetrieveCouncilSuburbsCount(int councilID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from sub in ent.Suburb
                        where sub.CouncilID == councilID
                        select sub;
            return query.Count();
        }

        #endregion




        public void insertNewActivityReference(DataSetComponent.ActivityReferenceCodeRow actRefDr)
        {
            ISDEntities ent = new ISDEntities();

            ActivityReferenceCode refCode = new ActivityReferenceCode();
            ObjectHandler.CopyPropertyValues(actRefDr, refCode);
            ent.ActivityReferenceCode.Add(refCode);

            ent.SaveChanges();
        }

        public void insertNewActivitiesReference(DataSetComponent.ActivityReferenceCodeDataTable actRefDT)
        {
            ISDEntities ent = new ISDEntities();

            foreach (var actRefDR in actRefDT)
            {
                ActivityReferenceCode refCode = new ActivityReferenceCode();
                ObjectHandler.CopyPropertyValues(actRefDR, refCode);
                ent.ActivityReferenceCode.Add(refCode);
            }
            ent.SaveChanges();
        }

        public void SaveAttendanceRecords(DataSetComponent.ActivityUserAttendanceDataTable dt)
        {
            ISDEntities ent = new ISDEntities();

            foreach (var dr in dt)
            {
                ActivityUserAttendance act = new ActivityUserAttendance();
                ObjectHandler.CopyPropertyValues(dr, act);
                ent.ActivityUserAttendance.Add(act);
            }
            ent.SaveChanges();
        }
        public void SaveAttendanceRecord(DataSetComponent.ActivityUserAttendanceRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ActivityUserAttendance act = new ActivityUserAttendance();
            ObjectHandler.CopyPropertyValues(dr, act);
            ent.ActivityUserAttendance.Add(act);

            ent.SaveChanges();
        }

        public void UpdateWebConfiguration(DataSetComponent.WebConfigurationRow dr)
        {
            ISDEntities ent = new ISDEntities();
            var query = from w in ent.WebConfiguration
                        select w;

            WebConfiguration conf = query.FirstOrDefault();
            dr.ID = conf.ID;
            ObjectHandler.CopyPropertyValues(dr, conf);
            ent.SaveChanges();
        }

        public DataSetComponent.ActivityUserAttendanceDataTable RetrieveUnprocessedAttendanceRecords()
        {
            ISDEntities ent = new ISDEntities();

            var query = (from r in ent.ActivityUserAttendance
                         where r.Processed == false
                         select r).AsEnumerable();

            if (query != null)
            {
                var dt = new DataSetComponent.ActivityUserAttendanceDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;

        }
        public DataSetComponent.ActivityUserAttendanceDataTable RetrieveAttendanceRecords()
        {
            ISDEntities ent = new ISDEntities();

            var query = (from r in ent.ActivityUserAttendance
                         select r).AsEnumerable();

            if (query != null)
            {
                var dt = new DataSetComponent.ActivityUserAttendanceDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;

        }

        public void UpdateAttendanceRecords(DataSetComponent.ActivityUserAttendanceDataTable unProcAttendance)
        {
            ISDEntities ent = new ISDEntities();
            List<int> procAtts = unProcAttendance.Select(x => x.ID).ToList();
            List<ActivityUserAttendance> auts = (from r in ent.ActivityUserAttendance
                                                 where r.Processed == false && procAtts.Contains(r.ID)
                                                 select r).ToList();

            if (auts != null)
            {
                foreach (var att in unProcAttendance)
                {
                    ActivityUserAttendance aut = auts.Where(x => x.ID == att.ID).FirstOrDefault();
                    if (aut != null)
                        ObjectHandler.CopyPropertyValues(att, aut);

                }
                ent.SaveChanges();
            }



        }

        public void AddAwardPointsToUsers(Dictionary<String, DataSetComponent.UserRewardLogRow> dicRewardLogs)
        {
            ISDEntities ent = new ISDEntities();
            IEnumerable<String> awards = dicRewardLogs.Keys;

            List<UserReward> rewards = (from ur in ent.UserReward
                                        where awards.Contains(ur.UserID)
                                        select ur).ToList();

            foreach (var dicRewardLog in dicRewardLogs)
            {
                UserReward reward = rewards.Where(x => x.UserID == dicRewardLog.Key).FirstOrDefault();
                if (reward != null)
                {
                    reward.RewardPoint = reward.RewardPoint + dicRewardLog.Value.PointValue;
                    reward.BonusPoint = reward.BonusPoint + dicRewardLog.Value.BonusPoint;
                    dicRewardLog.Value.UserRewardID = reward.ID;
                }
                else
                {
                    int userRewardID = 0;

                    UserReward user = new UserReward();
                    user.UserID = dicRewardLog.Key;
                    user.RewardPoint = dicRewardLog.Value.PointValue;
                    user.BonusPoint = dicRewardLog.Value.BonusPoint;
                    user.Status = 1;
                    ent.UserReward.Add(user);

                    dicRewardLog.Value.UserRewardID = userRewardID;
                }
            }
            ent.SaveChanges();
        }

        private void CreateNewRewardUser(String UserID, DataSetComponent.UserRewardLogRow userRewardLogRow, out int rewardID)
        {
            ISDEntities ent = new ISDEntities();
            UserReward user = new UserReward();

            user.UserID = UserID;
            user.RewardPoint = userRewardLogRow.PointValue;
            user.BonusPoint = userRewardLogRow.BonusPoint;
            user.Status = 1;

            ent.UserReward.Add(user);
            ent.SaveChanges();
            rewardID = user.ID;
        }

        public void AddAwardPointsToUser(DataSetComponent.UserRewardLogRow userRewardLog)
        {
            ISDEntities ent = new ISDEntities();

            UserReward reward = (from ur in ent.UserReward
                                 where ur.ID == userRewardLog.UserRewardID
                                 select ur).FirstOrDefault();
            if (reward != null)
            {
                reward.RewardPoint = reward.RewardPoint + userRewardLog.PointValue;
                reward.BonusPoint = reward.BonusPoint + userRewardLog.BonusPoint;
            }
            ent.SaveChanges();
        }

        public void AddAwardPointsToUser(String userID, DataSetComponent.UserRewardLogRow userRewardLog)
        {
            ISDEntities ent = new ISDEntities();

            UserReward reward = (from ur in ent.UserReward
                                 where ur.UserID == userID
                                 select ur).FirstOrDefault();
            if (reward != null)
            {
                reward.RewardPoint = reward.RewardPoint + userRewardLog.PointValue;
                reward.BonusPoint = reward.BonusPoint + userRewardLog.BonusPoint;
            }
            else
            {
                UserReward user = new UserReward();
                user.UserID = userID;
                user.RewardPoint = userRewardLog.PointValue;
                user.BonusPoint = userRewardLog.BonusPoint;
                user.Status = 1;
                ent.UserReward.Add(user);
            }
            ent.SaveChanges();
        }

        public void SaveRewardLogs(Dictionary<String, DataSetComponent.UserRewardLogRow> dicRewardLogs)
        {
            ISDEntities ent = new ISDEntities();
            foreach (var dicRewardLog in dicRewardLogs)
            {
                UserRewardLog rew = new UserRewardLog();
                ObjectHandler.CopyPropertyValues(dicRewardLog.Value, rew);
                ent.UserRewardLog.Add(rew);
            }
            ent.SaveChanges();
        }


        public void ValidateUserRewards(List<String> userList)
        {
            ISDEntities ent = new ISDEntities();
            List<string> UserRewardsList = ent.UserReward.Select(x => x.UserID).ToList();
            List<string> userWORewards = userList.Where(x => !UserRewardsList.Contains(x)).ToList();

            foreach (var userString in userWORewards)
            {
                UserReward UR = new UserReward();
                UR.UserID = userString;
                UR.RewardPoint = 0;
                UR.BonusPoint = 0;
                UR.RedeemedtPoint = 0;
                UR.Status = 1;
                ent.UserReward.Add(UR);
            }
            ent.SaveChanges();
        }

        #region ActivityRegistration

        public void CreateActivityContactDetail(DataSetComponent.ActivityContactDetailRow activityContactDetailDR)
        {
            ISDEntities ent = new ISDEntities();
            ActivityContactDetail actCon = new ActivityContactDetail();


            actCon.ActivityID = activityContactDetailDR.ActivityID;
            actCon.Title = activityContactDetailDR.Title;
            actCon.Username = activityContactDetailDR.Username;
            actCon.FirstName = activityContactDetailDR.FirstName;
            actCon.MiddleName = activityContactDetailDR.MiddleName;
            actCon.LastName = activityContactDetailDR.LastName;
            actCon.Email = activityContactDetailDR.Email;
            actCon.Address = activityContactDetailDR.Address;
            actCon.SuburbID = activityContactDetailDR.SuburbID;
            actCon.PostCode = activityContactDetailDR.PostCode;
            actCon.PhoneNumber = activityContactDetailDR.PhoneNumber;
            actCon.MobileNumber = activityContactDetailDR.MobileNumber;
            actCon.StateID = activityContactDetailDR.StateID;

            actCon.AltFirstName = activityContactDetailDR.AltFirstName;
            actCon.AltMiddleName = activityContactDetailDR.AltMiddleName;
            actCon.AltLastName = activityContactDetailDR.AltLastName;
            actCon.AltEmail = activityContactDetailDR.AltEmail;
            actCon.AltAddress = activityContactDetailDR.AltAddress;
            actCon.AltSuburbID = activityContactDetailDR.AltSuburbID;
            actCon.AltPostCode = activityContactDetailDR.AltPostCode;
            actCon.AltPhoneNumber = activityContactDetailDR.AltPhoneNumber;
            actCon.AltMobileNumber = activityContactDetailDR.AltMobileNumber;
            actCon.AltStateID = activityContactDetailDR.AltStateID;

            ent.ActivityContactDetail.Add(actCon);
            ent.SaveChanges();
        }


        public void CreateActivityGrouping(DataSetComponent.ActivityGroupingRow activityGroupDR)
        {
            ISDEntities ent = new ISDEntities();

            ActivityGrouping actGroup = new ActivityGrouping();

            actGroup.ActivityID = activityGroupDR.ActivityID;
            actGroup.forMale = activityGroupDR.forMale;
            actGroup.forFemale = activityGroupDR.forFemale;

            actGroup.forChildren = activityGroupDR.forChildren;
            actGroup.AgeFrom = activityGroupDR.AgeFrom;
            actGroup.AgeTo = activityGroupDR.AgeTo;

            ent.ActivityGrouping.Add(actGroup);
            ent.SaveChanges();
        }

        public void CreateActivities(DataSetComponent.ActivityRow activityDR, out int activityID)
        {
            ISDEntities ent = new ISDEntities();
            Activity act = new Activity();

            act.ActivityCode = activityDR.ActivityCode;
            act.ProviderID = activityDR.ProviderID;
            act.Name = activityDR.Name;
            act.ShortDescription = activityDR.ShortDescription;
            act.FullDescription = activityDR.FullDescription;
            act.CategoryID = activityDR.CategoryID;
            act.Price = activityDR.Price;
            act.ExpiryDate = activityDR.ExpiryDate;
            act.ActivityType = activityDR.ActivityType;
            act.eligibilityDescription = activityDR.eligibilityDescription;
            act.Website = activityDR.Website;
            act.Status = activityDR.Status;

            act.CreatedBy = activityDR.CreatedBy;
            act.CreatedDateTime = activityDR.CreatedDateTime;
            act.ModifiedBy = activityDR.ModifiedBy;
            act.ModifiedDateTime = activityDR.ModifiedDateTime;

            ent.Activity.Add(act);
            ent.SaveChanges();

            activityID = act.ID;
        }
        #endregion

        #region ImageViewer

        public DataSetComponent.ActivityImageDetailRow RetrieveProductImage(int activityID, int imageID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID && i.ID == imageID
                        select i;

            DataSetComponent.ActivityImageDetailRow dr = new DataSetComponent.ActivityImageDetailDataTable().NewActivityImageDetailRow();
            ActivityImageDetail pi = query.FirstOrDefault();
            if (pi != null)
                ObjectHandler.CopyPropertyValues(pi, dr);

            return dr;
        }

        public DataSetComponent.WebConfigurationRow RetrieveWebImage()
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.WebConfiguration
                        select i;
            WebConfiguration web = query.FirstOrDefault();
            DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
            if (web != null)
            {
                ObjectHandler.CopyPropertyValues(web, dr);
                return dr;
            }
            else return null;

        }

        public DataSetComponent.ActivityImageDetailRow RetrieveActivityPrimaryImage(int activityID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.ActivityImageDetail
                        where i.ActivityID == activityID && i.isPrimaryImage == true
                        select i;

            DataSetComponent.ActivityImageDetailRow dr = new DataSetComponent.ActivityImageDetailDataTable().NewActivityImageDetailRow();
            ActivityImageDetail pi = query.FirstOrDefault();
            if (pi != null)
                ObjectHandler.CopyPropertyValues(pi, dr);
            else
            {
                //dr.ProductID = 0;
                //dr.ImageID = 0;
                return null;
            }
            return dr;
        }

        /*
        public bool CheckImageBanner()
        {
            ISDEntities ent = new ISDEntities();
            var query = from w in ent.WebConfiguration
                        select w;
            if (query.FirstOrDefault() != null)
            {
                DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
                if (!dr.IsBannerImageNameNull())
                    return true;
                else return false;
            }
            else return false;
        }

        public bool CheckImageLogo()
        {
            ISDEntities ent = new ISDEntities();
            var query = from w in ent.WebConfiguration
                        select w;

            if (query.FirstOrDefault() != null)
            {
                DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
                if (!dr.IsLogoImageNameNull())
                    return true;
                else return false;
            }
            else return false;

        }
        */
        #endregion

        #region ActivityListing
        /*ActivityView contains value used in reports only, ActivityExplorer contains complete 
         *value used in activity listing and details. use activity ID to retrieve the activities
         *you want to be show. int[] ActivityID. */

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivitiesbySearchPhrase(String providerID, string stFrom, string stTo, string tmFrom, string tmTo, string searchKey, int ageFrom, int ageTo, string suburbID, int categoryID, int startIndex, int amount, string sortExpression, string MonFilter, string TueFilter, string WedFilter, string ThursFilter, string FriFilter, string SatFilter, string SunFilter)
        {

            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query;
            //filtering ProviderID  
            if (providerID != String.Empty)
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID &&
                        (a.Status == (int)SystemConstants.ActivityStatus.Active ||
                        a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby a.ID
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.Status == (int)SystemConstants.ActivityStatus.Active ||
                        a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby a.ID
                        select a;
            }
            //we are now creating a dictionary for matching query

            Dictionary<string, HashSet<int>> ActDictionary = new Dictionary<string, HashSet<int>>();
            foreach (var activity in query)
            {
                activity.Name = Regex.Replace(activity.Name, @"[!@#-;,:$%_]", "");
                activity.ShortDescription = Regex.Replace(activity.ShortDescription, @"[!@#-;,:$%_]", "");

                string[] actTitle = activity.Name.Trim().Split();
                foreach (var word in actTitle)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
                string[] keywords = activity.Keywords.Trim().Split(';');
                foreach (var word in keywords)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
                string[] actDesc = activity.ShortDescription.Trim().Split();
                foreach (var word in actDesc)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
            }


            HashSet<string> keywordsMPs = new HashSet<string>();
            foreach (var word in Keywords)
            {
                DoubleMetaphone mp = new DoubleMetaphone(word);
                if (mp.PrimaryKey != null)
                    keywordsMPs.Add(mp.PrimaryKey);
                if (mp.AlternateKey != null)
                    keywordsMPs.Add(mp.AlternateKey);
            }
            HashSet<int> matchesAct = new HashSet<int>();
            foreach (var keywordsMP in keywordsMPs)
            {
                HashSet<int> matches = new HashSet<int>();
                ActDictionary.TryGetValue(keywordsMP, out matches);
                if (matches != null)
                {
                    matchesAct.UnionWith(matches);
                }
            }
            IEnumerable<v_ActivityExplorer> Suggestions = query.Where(x => matchesAct.Contains(x.ID));

            //START FILTERING ACTIVITY BY CATEGORY
            if (categoryID != 0)
            {
                Suggestions = from a in Suggestions
                              where a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID
                              select a;
            }

            //START FILTERING ACTIVITY BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                Suggestions = from a in Suggestions
                              where a.AgeFrom <= ageFrom && a.AgeTo >= ageTo
                              select a;
            }

            //START FILTERING ACTIVITY BY LOCATION
            if (!string.IsNullOrEmpty(suburbID) && suburbID != "0")
            {

                string[] suburbs = suburbID.Split('|');
                List<int> suburbsInt = new List<int>();
                foreach (var suburb in suburbs)
                {
                    var suburbInt = Convert.ToInt32(suburb);
                    suburbsInt.Add(suburbInt);
                }
                Suggestions = from a in Suggestions
                              from s in suburbsInt
                              where a.SuburbID == s
                              select a;
            }


            if (sortExpression == SystemConstants.sortName)
                Suggestions = Suggestions.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                Suggestions = Suggestions.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                Suggestions = Suggestions.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                Suggestions = Suggestions.OrderByDescending(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortNameDesc)
                Suggestions = Suggestions.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                Suggestions = Suggestions.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                Suggestions = Suggestions.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                Suggestions = Suggestions.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                Suggestions = Suggestions.OrderByDescending(row => row.ActivityType);

            //START FILTERING ACTIVITY BY MATCHING DATE & DAY & TIME
            bool mon = Convert.ToBoolean(MonFilter); bool fri = Convert.ToBoolean(FriFilter);
            bool tue = Convert.ToBoolean(TueFilter); bool sat = Convert.ToBoolean(SatFilter);
            bool wed = Convert.ToBoolean(WedFilter); bool sun = Convert.ToBoolean(SunFilter);
            bool thu = Convert.ToBoolean(ThursFilter);
            DateTime dtFrom = Convert.ToDateTime(stFrom);
            DateTime dtTo = Convert.ToDateTime(stTo);
            TimeSpan timeFrom = Convert.ToDateTime(tmFrom).TimeOfDay;
            TimeSpan timeTo = Convert.ToDateTime(tmTo).TimeOfDay;

            if ((dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate) || (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay) || (!mon || !tue || !wed || !thu || !fri || !sat || !sun))
            {
                HashSet<int> actScheds = new HashSet<int>(ent.ActivitySchedule.Select(x => x.ActivityID));
                HashSet<int> acts = new HashSet<int>(Suggestions.Select(x => x.ID));
                HashSet<int> ActsWISched = new HashSet<int>(acts.Where(x => actScheds.Contains(x)));
                HashSet<int> ActsWOSched = new HashSet<int>(acts.Where(x => !actScheds.Contains(x)));
                DataSetComponent.ActivityScheduleDataTable Schedsdt = RetrieveActivitiesSchedulesbyIDs(ActsWISched);

                if (dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDate(Schedsdt, dtFrom, dtTo);
                }

                if (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesTime(Schedsdt, timeFrom, timeTo);
                }


                if (!mon || !tue || !wed || !thu || !fri || !sat || !sun)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDay(Schedsdt, mon, tue, wed, thu, fri, sat, sun);
                }



                if (ActsWISched != null)
                {
                    var filteredSuggestions = Suggestions.Where(x => ActsWISched.Contains(x.ID));
                    if (ActsWOSched != null)
                        Suggestions = Suggestions.Where(x => ActsWOSched.Contains(x.ID));

                    if (sortExpression == SystemConstants.sortName)
                        filteredSuggestions = filteredSuggestions.OrderBy(row => row.Name);
                    else if (sortExpression == SystemConstants.sortLatest)
                        filteredSuggestions = filteredSuggestions.OrderBy(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortExpiry)
                        filteredSuggestions = filteredSuggestions.OrderBy(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortPrice)
                        filteredSuggestions = filteredSuggestions.OrderByDescending(row => row.IsPaid);
                    else if (sortExpression == SystemConstants.sortNameDesc)
                        filteredSuggestions = filteredSuggestions.OrderByDescending(row => row.Name);
                    else if (sortExpression == SystemConstants.sortLatestDesc)
                        filteredSuggestions = filteredSuggestions.OrderByDescending(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortExpiryDesc)
                        filteredSuggestions = filteredSuggestions.OrderByDescending(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortPriceDesc)
                        filteredSuggestions = filteredSuggestions.OrderBy(row => row.IsPaid);
                    else if (sortExpression == SystemConstants.sortExpiryDesc)
                        filteredSuggestions = filteredSuggestions.OrderByDescending(row => row.ActivityType);

                    Suggestions = filteredSuggestions.Select(x => x).Concat(Suggestions.Select(y => y));
                }

            }
            Suggestions = Suggestions.Skip(startIndex).Take(amount).AsEnumerable();

            if (Suggestions != null && Suggestions.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(Suggestions, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else return null;

        }

        //public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesReportbySearchPhrase(String providerID, string stFrom, string stTo, string searchKey, string sortExpression)
        //{
        //    //splitting keywords as keyword can be a multiple words
        //    String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

        //    ISDEntities ent = new ISDEntities();
        //    IQueryable<v_ActivityView> query;
        //    //filtering ProviderID  
        //    if (providerID != String.Empty)
        //    {
        //        query = from a in ent.v_ActivityView
        //                where a.ProviderID == providerID &&
        //                (a.Status == (int)SystemConstants.ActivityStatus.Active ||
        //                a.Status == (int)SystemConstants.ActivityStatus.WillExpire)
        //                orderby a.ID
        //                select a;
        //    }
        //    else
        //    {
        //        query = from a in ent.v_ActivityView
        //                where (a.Status == (int)SystemConstants.ActivityStatus.Active ||
        //                a.Status == (int)SystemConstants.ActivityStatus.WillExpire)
        //                orderby a.ID
        //                select a;
        //    }


        //    var Suggestions = (from a in query
        //                       from w in Keywords
        //                       where a.Name.ToUpper().Contains(w.ToUpper()) || a.ShortDescription.ToUpper().Contains(w.ToUpper())
        //                       || a.Keywords.ToUpper().Contains(w.ToUpper()) && (a.Status == (int)SystemConstants.ActivityStatus.Active
        //                           || a.Status == (int)SystemConstants.ActivityStatus.WillExpire)
        //                       orderby sortExpression
        //                       select a).Distinct().ToArray();

        //    IEnumerable<v_ActivityView> activities = Suggestions.AsEnumerable();

        //    if (sortExpression == SystemConstants.sortName)
        //        activities = activities.OrderBy(row => row.Name);
        //    else if (sortExpression == SystemConstants.sortLatest)
        //        activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
        //    else if (sortExpression == SystemConstants.sortExpiry)
        //        activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
        //    else if (sortExpression == SystemConstants.sortNameDesc)
        //        activities = activities.OrderByDescending(row => row.Name);
        //    else if (sortExpression == SystemConstants.sortLatestDesc)
        //        activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
        //    else if (sortExpression == SystemConstants.sortExpiryDesc)
        //        activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);

        //    DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
        //    ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

        //    DateTime dtFrom = Convert.ToDateTime(stFrom);
        //    DateTime dtTo = Convert.ToDateTime(stTo);

        //    if (dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate)
        //    {
        //        HashSet<int> actIDID = new HashSet<int>();
        //        foreach (var dr in dt)
        //            actIDID.Add(dr.ActivityID);

        //        if (actIDID.Count != 0)
        //        {
        //            HashSet<int> FilteredID = FilterActivitiesDate(actIDID, dtFrom, dtTo);
        //            dt = RetrieveActivityViewsbyIDs(FilteredID, sortExpression);
        //            return dt;
        //        }
        //    }
        //    return dt;
        //}

        public int RetrieveProviderActivitiesbySearchPhraseCount(String providerID, string stFrom, string stTo, string tmFrom, string tmTo, int ageFrom, int ageTo, string suburbID, int categoryID, string searchKey, string MonFilter, string TueFilter, string WedFilter, string ThursFilter, string FriFilter, string SatFilter, string SunFilter)
        {

            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query;
            //filtering ProviderID  
            if (providerID != String.Empty)
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID &&
                        (a.Status == (int)SystemConstants.ActivityStatus.Active ||
                        a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby a.ID
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.Status == (int)SystemConstants.ActivityStatus.Active ||
                        a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby a.ID
                        select a;
            }
            //we are now creating a dictionary for matching query

            Dictionary<string, HashSet<int>> ActDictionary = new Dictionary<string, HashSet<int>>();
            foreach (var activity in query)
            {
                activity.Name = Regex.Replace(activity.Name, @"[!@#-;,:$%_]", "");
                activity.ShortDescription = Regex.Replace(activity.ShortDescription, @"[!@#-;,:$%_]", "");

                string[] actTitle = activity.Name.Trim().Split();
                foreach (var word in actTitle)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
                string[] keywords = activity.Keywords.Trim().Split(';');
                foreach (var word in keywords)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
                string[] actDesc = activity.ShortDescription.Trim().Split();
                foreach (var word in actDesc)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    HashSet<int> savedAct = new HashSet<int>();
                    ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                    if (savedAct == null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary[mp.PrimaryKey] = savedAct;
                    }
                    savedAct.Add(activity.ID);

                    if (mp.AlternateKey != null)
                    {
                        savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.AlternateKey] = savedAct;
                        }
                        savedAct.Add(activity.ID);
                    }
                }
            }


            HashSet<string> keywordsMPs = new HashSet<string>();
            foreach (var word in Keywords)
            {
                DoubleMetaphone mp = new DoubleMetaphone(word);
                if (mp.PrimaryKey != null)
                    keywordsMPs.Add(mp.PrimaryKey);
                if (mp.AlternateKey != null)
                    keywordsMPs.Add(mp.AlternateKey);
            }
            HashSet<int> matchesAct = new HashSet<int>();
            foreach (var keywordsMP in keywordsMPs)
            {
                HashSet<int> matches = new HashSet<int>();
                ActDictionary.TryGetValue(keywordsMP, out matches);
                if (matches != null)
                {
                    matchesAct.UnionWith(matches);
                }
            }
            IEnumerable<v_ActivityExplorer> Suggestions = query.Where(x => matchesAct.Contains(x.ID));

            //START FILTERING ACTIVITY BY CATEGORY
            if (categoryID != 0)
            {
                Suggestions = from a in Suggestions
                              where a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID
                              select a;
            }

            //START FILTERING ACTIVITY BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                Suggestions = from a in Suggestions
                              where a.AgeFrom <= ageFrom && a.AgeTo >= ageTo
                              select a;
            }

            //START FILTERING ACTIVITY BY LOCATION
            if (!string.IsNullOrEmpty(suburbID) && suburbID != "0")
            {

                string[] suburbs = suburbID.Split('|');
                List<int> suburbsInt = new List<int>();
                foreach (var suburb in suburbs)
                {
                    var suburbInt = Convert.ToInt32(suburb);
                    suburbsInt.Add(suburbInt);
                }
                Suggestions = from a in Suggestions
                              from s in suburbsInt
                              where a.SuburbID == s
                              select a;
            }

            //START FILTERING ACTIVITY BY MATCHING DATE & DAY & TIME
            bool mon = Convert.ToBoolean(MonFilter); bool fri = Convert.ToBoolean(FriFilter);
            bool tue = Convert.ToBoolean(TueFilter); bool sat = Convert.ToBoolean(SatFilter);
            bool wed = Convert.ToBoolean(WedFilter); bool sun = Convert.ToBoolean(SunFilter);
            bool thu = Convert.ToBoolean(ThursFilter);
            DateTime dtFrom = Convert.ToDateTime(stFrom);
            DateTime dtTo = Convert.ToDateTime(stTo);
            TimeSpan timeFrom = Convert.ToDateTime(tmFrom).TimeOfDay;
            TimeSpan timeTo = Convert.ToDateTime(tmTo).TimeOfDay;

            if ((dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate) || (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay) || (!mon || !tue || !wed || !thu || !fri || !sat || !sun))
            {
                HashSet<int> actScheds = new HashSet<int>(ent.ActivitySchedule.Select(x => x.ActivityID));
                HashSet<int> acts = new HashSet<int>(Suggestions.Select(x => x.ID));
                HashSet<int> ActsWISched = new HashSet<int>(acts.Where(x => actScheds.Contains(x)));
                HashSet<int> ActsWOSched = new HashSet<int>(acts.Where(x => !actScheds.Contains(x)));
                DataSetComponent.ActivityScheduleDataTable Schedsdt = RetrieveActivitiesSchedulesbyIDs(ActsWISched);

                if (dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDate(Schedsdt, dtFrom, dtTo);
                }

                if (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesTime(Schedsdt, timeFrom, timeTo);
                }


                if (!mon || !tue || !wed || !thu || !fri || !sat || !sun)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDay(Schedsdt, mon, tue, wed, thu, fri, sat, sun);
                }


                if (ActsWISched != null)
                {
                    var filteredSuggestions = Suggestions.Where(x => ActsWISched.Contains(x.ID));
                    if (ActsWOSched != null)
                        Suggestions = Suggestions.Where(x => ActsWOSched.Contains(x.ID));

                    Suggestions = filteredSuggestions.Select(x => x).Concat(Suggestions.Select(y => y));
                }

            }
            Suggestions = Suggestions.AsEnumerable();

            if (Suggestions != null && Suggestions.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(Suggestions, dt, LoadOption.OverwriteChanges);
                return dt.Count;
            }
            else return 0;

        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesReport(String providerID, string stFrom, string stTo, int categoryID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityView> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }



            IEnumerable<v_ActivityView> activities = query.AsEnumerable();

            if (activities != null)
            {
                if (sortExpression == SystemConstants.sortName)
                    activities = activities.OrderBy(row => row.Name);
                else if (sortExpression == SystemConstants.sortLatest)
                    activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
                else if (sortExpression == SystemConstants.sortExpiry)
                    activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
                else if (sortExpression == SystemConstants.sortNameDesc)
                    activities = activities.OrderByDescending(row => row.Name);
                else if (sortExpression == SystemConstants.sortLatestDesc)
                    activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);

                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }

            else
                return null;


        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivities(String providerID, string stFrom, string stTo, string tmFrom, string tmTo, int categoryID, int ageFrom, int ageTo, string suburbID, int startIndex, int amount, string sortExpression, string MonFilter, string TueFilter, string WedFilter, string ThursFilter, string FriFilter, string SatFilter, string SunFilter)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.SecondaryCategoryID1 == categoryID ||
                                        a.SecondaryCategoryID2 == categoryID ||
                                        a.SecondaryCategoryID3 == categoryID ||
                                        a.SecondaryCategoryID4 == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                                   && (a.Status == (int)SystemConstants.ActivityStatus.Active
                                   || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.SecondaryCategoryID1 == categoryID ||
                                   a.SecondaryCategoryID2 == categoryID ||
                                   a.SecondaryCategoryID3 == categoryID ||
                                   a.SecondaryCategoryID4 == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID)
                                   && (a.Status == (int)SystemConstants.ActivityStatus.Active
                                   || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true

                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.Status == (int)SystemConstants.ActivityStatus.Active
                        || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        orderby sortExpression
                        select a;
            }

            //START FILTERING ACTIVITY BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                query = from a in query
                        where a.AgeFrom <= ageFrom && a.AgeTo >= ageTo
                        orderby sortExpression
                        select a;
            }

            //START FILTERING ACTIVITY BY LOCATION
            if (!string.IsNullOrEmpty(suburbID) && suburbID != "0")
            {

                string[] suburbs = suburbID.Split('|');
                List<int> suburbsInt = new List<int>();
                foreach (var suburb in suburbs)
                {
                    var suburbInt = Convert.ToInt32(suburb);
                    suburbsInt.Add(suburbInt);
                }
                query = from a in query
                        from s in suburbsInt
                        where a.SuburbID == s
                        orderby sortExpression
                        select a;
            }

            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);

            //START FILTERING ACTIVITY BY MATCHING DATE & DAY
            bool mon = Convert.ToBoolean(MonFilter); bool fri = Convert.ToBoolean(FriFilter);
            bool tue = Convert.ToBoolean(TueFilter); bool sat = Convert.ToBoolean(SatFilter);
            bool wed = Convert.ToBoolean(WedFilter); bool sun = Convert.ToBoolean(SunFilter);
            bool thu = Convert.ToBoolean(ThursFilter);
            DateTime dtFrom = Convert.ToDateTime(stFrom);
            DateTime dtTo = Convert.ToDateTime(stTo);
            TimeSpan timeFrom = Convert.ToDateTime(tmFrom).TimeOfDay;
            TimeSpan timeTo = Convert.ToDateTime(tmTo).TimeOfDay;

            if ((dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate) || (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay) || (!mon || !tue || !wed || !thu || !fri || !sat || !sun))
            {
                HashSet<int> actScheds = new HashSet<int>(ent.ActivitySchedule.Select(x => x.ActivityID));
                HashSet<int> acts = new HashSet<int>(activities.Select(x => x.ID));
                HashSet<int> ActsWISched = new HashSet<int>(acts.Where(x => actScheds.Contains(x)));
                HashSet<int> ActsWOSched = new HashSet<int>(acts.Where(x => !actScheds.Contains(x)));
                DataSetComponent.ActivityScheduleDataTable Schedsdt = RetrieveActivitiesSchedulesbyIDs(ActsWISched);

                if (dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDate(Schedsdt, dtFrom, dtTo);
                    else return null;
                }
                if (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesTime(Schedsdt, timeFrom, timeTo);
                }

                if (!mon || !tue || !wed || !thu || !fri || !sat || !sun)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDay(Schedsdt, mon, tue, wed, thu, fri, sat, sun);
                    else return null;
                }

                if (ActsWISched != null)
                {
                    var filteredActivities = activities.Where(x => ActsWISched.Contains(x.ID));
                    if (ActsWOSched != null)
                        activities = activities.Where(x => ActsWOSched.Contains(x.ID));
                    if (sortExpression == SystemConstants.sortName)
                        filteredActivities = filteredActivities.OrderBy(row => row.Name);
                    else if (sortExpression == SystemConstants.sortLatest)
                        filteredActivities = filteredActivities.OrderBy(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortExpiry)
                        filteredActivities = filteredActivities.OrderBy(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortPrice)
                        filteredActivities = filteredActivities.OrderByDescending(row => row.IsPaid);
                    else if (sortExpression == SystemConstants.sortNameDesc)
                        filteredActivities = filteredActivities.OrderByDescending(row => row.Name);
                    else if (sortExpression == SystemConstants.sortLatestDesc)
                        filteredActivities = filteredActivities.OrderByDescending(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortExpiryDesc)
                        filteredActivities = filteredActivities.OrderByDescending(row => row.ModifiedDateTime);
                    else if (sortExpression == SystemConstants.sortPriceDesc)
                        filteredActivities = filteredActivities.OrderBy(row => row.IsPaid);
                    else if (sortExpression == SystemConstants.sortExpiryDesc)
                        filteredActivities = filteredActivities.OrderByDescending(row => row.ActivityType);

                    activities = filteredActivities.Select(x => x).Concat(activities.Select(y => y));
                }
            }
            activities = activities.Skip(startIndex).Take(amount).AsEnumerable();

            if (activities != null && activities.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else return null;
        }


        //public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivities(String providerID, int categoryID, string sortExpression)
        //{
        //    ISDEntities ent = new ISDEntities();

        //    //filtering ProviderID  
        //    IQueryable<v_ActivityExplorer> query;

        //    if (providerID != String.Empty && categoryID != 0)
        //    {
        //        query = from a in ent.v_ActivityExplorer
        //                where (a.ProviderID == providerID && (
        //                                a.CategoryID == categoryID ||
        //                                a.CategoryLevel1ParentID == categoryID ||
        //                                a.CategoryLevel2ParentID == categoryID))
        //                           && (a.Status == (int)SystemConstants.ActivityStatus.Active
        //                           || a.Status == (int)SystemConstants.ActivityStatus.WillExpire) && a.isApproved == true
        //                orderby sortExpression
        //                select a;
        //    }
        //    else if (providerID == String.Empty && categoryID != 0)
        //    {
        //        query = from a in ent.v_ActivityExplorer
        //                where (a.CategoryID == categoryID ||
        //                           a.CategoryLevel1ParentID == categoryID ||
        //                           a.CategoryLevel2ParentID == categoryID)
        //                           && (a.Status == (int)SystemConstants.ActivityStatus.Active
        //                           || a.Status == (int)SystemConstants.ActivityStatus.WillExpire) && a.isApproved == true

        //                orderby sortExpression
        //                select a;
        //    }
        //    else
        //    {
        //        query = from a in ent.v_ActivityExplorer
        //                where (a.Status == (int)SystemConstants.ActivityStatus.Active
        //                || a.Status == (int)SystemConstants.ActivityStatus.WillExpire) && a.isApproved == true
        //                orderby sortExpression
        //                select a;
        //    }

        //    IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();
        //    if (activities != null)
        //    {
        //        if (sortExpression == SystemConstants.sortName)
        //            activities = activities.OrderBy(row => row.Name);
        //        else if (sortExpression == SystemConstants.sortLatest)
        //            activities = activities.OrderBy(row => row.ModifiedDateTime);
        //        else if (sortExpression == SystemConstants.sortExpiry)
        //            activities = activities.OrderBy(row => row.ModifiedDateTime);
        //        else if (sortExpression == SystemConstants.sortNameDesc)
        //            activities = activities.OrderByDescending(row => row.Name);
        //        else if (sortExpression == SystemConstants.sortLatestDesc)
        //            activities = activities.OrderByDescending(row => row.ModifiedDateTime);
        //        else if (sortExpression == SystemConstants.sortExpiryDesc)
        //            activities = activities.OrderByDescending(row => row.ModifiedDateTime);
        //        DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
        //        ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

        //        return dt;
        //    }
        //    else return null;
        //}

        public int RetrieveProviderActivitiesCount(String providerID, string stFrom, string stTo, string tmFrom, string tmTo, int ageFrom, int ageTo, string suburbID, int categoryID, string MonFilter, string TueFilter, string WedFilter, string ThursFilter, string FriFilter, string SatFilter, string SunFilter)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.SecondaryCategoryID1 == categoryID ||
                                        a.SecondaryCategoryID2 == categoryID ||
                                        a.SecondaryCategoryID3 == categoryID ||
                                        a.SecondaryCategoryID4 == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                                   && (a.Status == (int)SystemConstants.ActivityStatus.Active
                                   || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.SecondaryCategoryID1 == categoryID ||
                                   a.SecondaryCategoryID2 == categoryID ||
                                   a.SecondaryCategoryID3 == categoryID ||
                                   a.SecondaryCategoryID4 == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID)
                                   && (a.Status == (int)SystemConstants.ActivityStatus.Active
                                   || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.Status == (int)SystemConstants.ActivityStatus.Active
                        || a.Status == (int)SystemConstants.ActivityStatus.WillExpire2) && a.isApproved == true
                        select a;
            }

            //START FILTERING ACTIVITY BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                query = from a in query
                        where a.AgeFrom <= ageFrom && a.AgeTo >= ageTo
                        select a;
            }

            //START FILTERING ACTIVITY BY LOCATION
            if (!string.IsNullOrEmpty(suburbID) && suburbID != "0")
            {

                string[] suburbs = suburbID.Split('|');
                List<int> suburbsInt = new List<int>();
                foreach (var suburb in suburbs)
                {
                    var suburbInt = Convert.ToInt32(suburb);
                    suburbsInt.Add(suburbInt);
                }
                query = from a in query
                        from s in suburbsInt
                        where a.SuburbID == s
                        select a;
            }

            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();


            //START FILTERING ACTIVITY BY MATCHING DATE & DAY
            bool mon = Convert.ToBoolean(MonFilter); bool fri = Convert.ToBoolean(FriFilter);
            bool tue = Convert.ToBoolean(TueFilter); bool sat = Convert.ToBoolean(SatFilter);
            bool wed = Convert.ToBoolean(WedFilter); bool sun = Convert.ToBoolean(SunFilter);
            bool thu = Convert.ToBoolean(ThursFilter);
            DateTime dtFrom = Convert.ToDateTime(stFrom);
            DateTime dtTo = Convert.ToDateTime(stTo);
            TimeSpan timeFrom = Convert.ToDateTime(tmFrom).TimeOfDay;
            TimeSpan timeTo = Convert.ToDateTime(tmTo).TimeOfDay;

            if ((dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate) || (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay) || (!mon || !tue || !wed || !thu || !fri || !sat || !sun))
            {
                HashSet<int> actScheds = new HashSet<int>(ent.ActivitySchedule.Select(x => x.ActivityID));
                HashSet<int> acts = new HashSet<int>(activities.Select(x => x.ID));
                HashSet<int> ActsWISched = new HashSet<int>(acts.Where(x => actScheds.Contains(x)));
                HashSet<int> ActsWOSched = new HashSet<int>(acts.Where(x => !actScheds.Contains(x)));
                DataSetComponent.ActivityScheduleDataTable Schedsdt = RetrieveActivitiesSchedulesbyIDs(ActsWISched);

                if (dtFrom != SystemConstants.nodate && dtTo != SystemConstants.nodate)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDate(Schedsdt, dtFrom, dtTo);
                }

                if (!mon || !tue || !wed || !thu || !fri || !sat || !sun)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesDay(Schedsdt, mon, tue, wed, thu, fri, sat, sun);
                }
                if (timeFrom != SystemConstants.nodate.TimeOfDay && timeTo != SystemConstants.nodate.TimeOfDay)
                {
                    if (ActsWISched.Count != 0)
                        ActsWISched = FilterActivitiesTime(Schedsdt, timeFrom, timeTo);
                }


                if (ActsWISched != null)
                {
                    var filteredActivities = activities.Where(x => ActsWISched.Contains(x.ID));
                    if (ActsWOSched != null)
                        activities = activities.Where(x => ActsWOSched.Contains(x.ID));
                    activities = filteredActivities.Select(x => x).Concat(activities.Select(y => y));
                }
            }
            activities = activities.AsEnumerable();

            if (activities != null && activities.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt.Count;
            }
            else return 0;
        }

        public DataSetComponent.v_ActivityViewRow RetrieveActivityView(int ActivityID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;

            query = from a in ent.v_ActivityView
                    where a.ID == ActivityID
                    select a;

            if (query.Count() == 0)
                return null;
            IEnumerable<v_ActivityView> activities = query.AsEnumerable();


            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                DataSetComponent.v_ActivityViewRow dr = new DataSetComponent.v_ActivityViewDataTable().Newv_ActivityViewRow();

                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                foreach (var drow in dt)
                {
                    dr = drow;
                }
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveActivityExplorersbyIDs(int[] activityID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;

            query = from a in ent.v_ActivityExplorer
                    from id in activityID
                    where a.ID.ToString().Contains(activityID.ToString())
                    select a;

            if (query.Count() == 0)
                return null;
            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);

            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);

            activities = activities.Skip(startIndex).Take(amount).AsEnumerable();



            if (activities != null && activities.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;

        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveActivityExplorersbyIDs(HashSet<int> activityID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;

            query = from a in ent.v_ActivityExplorer
                    where activityID.Contains(a.ID)
                    select a;
            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (activities != null && activities.Count() != 0)
            {
                if (sortExpression == SystemConstants.sortName)
                    activities = activities.OrderBy(row => row.Name);
                else if (sortExpression == SystemConstants.sortLatest)
                    activities = activities.OrderBy(row => row.ModifiedDateTime);
                else if (sortExpression == SystemConstants.sortExpiry)
                    activities = activities.OrderBy(row => row.ModifiedDateTime);
                else if (sortExpression == SystemConstants.sortNameDesc)
                    activities = activities.OrderByDescending(row => row.Name);
                else if (sortExpression == SystemConstants.sortLatestDesc)
                    activities = activities.OrderByDescending(row => row.ModifiedDateTime);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    activities = activities.OrderByDescending(row => row.ModifiedDateTime);

                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;

        }
        public DataSetComponent.v_ActivityExplorerDataTable RetrieveActivityExplorersbyIDs(HashSet<int> activityID, int startIndex, int Amount)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;

            query = from a in ent.v_ActivityExplorer
                    where activityID.Contains(a.ID)
                    select a;
            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (activities != null && activities.Count() != 0)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;

        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveActivityViewsbyIDs(HashSet<int> activityID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;

            query = from a in ent.v_ActivityView
                    where activityID.Contains(a.ID)
                    select a;



            if (query.Count() == 0)
                return null;
            IEnumerable<v_ActivityView> activities = query.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);

            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;

        }

        public int RetrieveActivityExplorersbyIDsCount(HashSet<int> activityID)
        {
            if (activityID == null)
                return 0;
            else
            {
                ISDEntities ent = new ISDEntities();
                IQueryable<v_ActivityExplorer> query = null;

                query = from a in ent.v_ActivityExplorer
                        where activityID.Contains(a.ID)
                        select a;

                return query.Count();
            }
        }

        public int RetrieveActivityExplorersbyIDsCount(int[] activityID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;

            query = from a in ent.v_ActivityExplorer
                    from id in activityID
                    where a.ID.ToString().Contains(activityID.ToString())
                    select a;

            return query.Count();
        }


        /*
        public DataSetComponent.v_ActivityViewDataTable RetrieveSearchActivities(string searchKey, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.v_ActivityView
                        where a.Name.Contains(searchKey) || a.CategoryName.Contains(searchKey)
                        || a.Name.Contains(searchKey) || a.CategoryLevel1ParentName.Contains(searchKey)
                        || a.CategoryLevel2ParentName.Contains(searchKey)
                        orderby sortExpression
                        select a;

            IEnumerable<v_ActivityView> activities = query.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveSearchActivitiesCount(string searchKey)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.v_ActivityView
                        where a.Name.Contains(searchKey) || a.CategoryName.Contains(searchKey)
                        || a.Name.Contains(searchKey) || a.CategoryLevel1ParentName.Contains(searchKey)
                        || a.CategoryLevel2ParentName.Contains(searchKey)
                        select a;

            return query.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveSearchProviderActivities(String providerID, String searchKey, int startIndex, int amount, string sortExpression)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityView> query = from a in ent.v_ActivityView
                                               where a.ProviderID == providerID
                                               orderby a.ID
                                               select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToUpper().Contains(w.ToUpper()) || a.ShortDescription.ToUpper().Contains(w.ToUpper())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_ActivityView> activities = Suggestions.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveSearchProviderActivitiesCount(String providerID, string searchKey)
        {
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ID  
            IQueryable<v_ActivityView> query = from a in ent.v_ActivityView
                                               where a.ProviderID == providerID
                                               select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.Contains(w)
                               select a).Distinct().ToArray();

            return Suggestions.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveActivityViews(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivityViewsCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        select a;
            }
            return query.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveActivityViewsFromActivitiesIDArray(String providerID, int[] activitiesID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivityViewsFromActivitiesIDArrayCount(String providerID, int[] activitiesID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty)
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        select a;
            }
            else
            {
                query = (from a in query
                         from w in activitiesID
                         where a.ID.ToString().Contains(w.ToString())
                         select a).Distinct().ToArray();
            }

            return query.Count();
        }

        public DataSetComponent.ActivityDataTable RetrieveActivities(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<Activity> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.ProviderID == providerID &&
                                        a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.Activity
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.ActivityDataTable dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivitiesCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<Activity> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.ProviderID == providerID &&
                                        a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.Activity
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }
            return query.Count();
        }
        */
        #endregion





        #region user-Provider

        public bool isEmailAddressExist(string emailaddress)
        {
            ISDEntities ent = new ISDEntities();

            var query = from u in ent.v_UserExplorer
                        where u.Email == emailaddress
                        select u;
            if (query != null && query.Count() != 0)
                return true;
            else return false;
        }

        public DataSetComponent.UserProfilesRow RetrieveMember(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from u in ent.UserProfiles
                        where u.UserID == userID
                        select u;

            UserProfiles user = query.FirstOrDefault();
            if (user != null)
            {
                var dr = new DataSetComponent.UserProfilesDataTable().NewUserProfilesRow();
                ObjectHandler.CopyPropertyValues(user, dr);
                return dr;
            }
            else return null;
        }

        public void UpdateProviderProviles(DataSetComponent.ProviderProfilesRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.UserProfiles
                        where ac.UserID == dr.UserID
                        select ac;

            UserProfiles provider = query.FirstOrDefault();
            if (provider != null)
            {
                ObjectHandler.CopyPropertyValues(dr, provider);
                ent.SaveChanges();
            }
        }

        public void InsertNewUserProfiles(DataSetComponent.UserProfilesRow dr)
        {
            ISDEntities ent = new ISDEntities();
            UserProfiles user = new UserProfiles();
            ObjectHandler.CopyPropertyValues(dr, user);

            ent.UserProfiles.Add(user);
            ent.SaveChanges();
        }

        #endregion

        #region emailtemplate

        public DataSetComponent.v_EmailExplorerRow RetrieveMailTemplate(int templateType)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.v_EmailExplorer
                        where e.EmailType == templateType
                        select e;

            if (query.FirstOrDefault() != null)
            {
                var dr = new DataSetComponent.v_EmailExplorerDataTable().Newv_EmailExplorerRow();
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
                return dr;
            }
            else return null;
        }

        #endregion

        #region Users
        public String RetrieveUserGuid(string username)
        {
            ISDEntities ent = new ISDEntities();
            var query = from u in ent.AspNetUsers
                        where u.UserName == username
                        select u;

            if (query.FirstOrDefault() != null)
                return query.FirstOrDefault().Id;
            else return String.Empty;

        }

        public String RetrieveUserStringbyEmailAddress(string EmailAddress)
        {
            if (string.IsNullOrEmpty(EmailAddress))
                return String.Empty;
            else
            {
                ISDEntities ent = new ISDEntities();

                var query = from u in ent.UserProfiles
                            where u.Email == EmailAddress
                            select u;

                if (query.FirstOrDefault() != null)
                    return query.FirstOrDefault().UserID;
                else
                {
                    var nextquery = from u in ent.ProviderProfiles
                                    where u.Email == EmailAddress
                                    select u;

                    if (nextquery.FirstOrDefault() != null)
                        return nextquery.FirstOrDefault().UserID;
                    else return String.Empty;
                }
            }
        }

        public string RetrieveUserNamebyEmailAddress(string EmailAddress)
        {
            if (string.IsNullOrEmpty(EmailAddress))
                return null;
            else
            {
                ISDEntities ent = new ISDEntities();

                var query = from u in ent.UserProfiles
                            where u.Email == EmailAddress
                            select u;

                if (query.FirstOrDefault() != null)
                    return query.FirstOrDefault().Username;
                else
                {
                    var nextquery = from u in ent.ProviderProfiles
                                    where u.Email == EmailAddress
                                    select u;

                    if (nextquery.FirstOrDefault() != null)
                        return nextquery.FirstOrDefault().Username;
                    else return null;
                }
            }
        }

        public DataSetComponent.UserReferenceDataTable RetrieveUserReferences()
        {
            ISDEntities ent = new ISDEntities();

            var query = from r in ent.UserReference
                        select r;

            if (query.AsEnumerable() != null)
            {
                var dt = new DataSetComponent.UserReferenceDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public void insertNewUserReference(DataSetComponent.UserReferenceRow drRef)
        {
            ISDEntities ent = new ISDEntities();
            UserReference user = new UserReference();
            ObjectHandler.CopyPropertyValues(drRef, user);

            ent.UserReference.Add(user);
            ent.SaveChanges();
        }

        public bool DeactivateUser(string usr, String userID, out string err)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.UserProfiles
                        where ac.UserID == userID && ac.Username == usr
                        select ac;

            UserProfiles user = query.FirstOrDefault();
            if (user != null)
            {
                if (user.AccountDeletion == true)
                {
                    err = "Account is deactivated";
                    return false;
                }
                else
                {
                    err = "Account is deactivated";
                    user.AccountDeletion = true;
                    ent.SaveChanges();
                    return true;
                }
            }
            else
            {
                var queryP = from ac in ent.ProviderProfiles
                             where ac.UserID == userID && ac.Username == usr
                             select ac;

                ProviderProfiles userp = queryP.FirstOrDefault();
                if (userp != null)
                {
                    if (userp.AccountDeletion == true)
                    {
                        err = "Account is deactivated";
                        return false;
                    }
                    else
                    {
                        err = "Account is deactivated";
                        userp.AccountDeletion = true;
                        ent.SaveChanges();
                        return true;
                    }
                }

                else
                {
                    err = "Unable to find user";
                    return false;
                }
            }
        }
        #endregion

        #region UserImage
        public void CreateUserImageInformation(DataSetComponent.UserImageRow dr)
        {
            ISDEntities ent = new ISDEntities();
            UserImage ii = new UserImage();
            ObjectHandler.CopyPropertyValues(dr, ii);

            ent.UserImage.Add(ii);
            ent.SaveChanges();
        }

        public void CreateUserImageInformation(DataSetComponent.UserImageRow dr, out int iiID)
        {
            ISDEntities ent = new ISDEntities();
            UserImage ii = new UserImage();
            ObjectHandler.CopyPropertyValues(dr, ii);

            ent.UserImage.Add(ii);
            ent.SaveChanges();
            iiID = ii.ID;
        }

        public void UpdateUserImageInformation(String userID, int iiID, DataSetComponent.UserImageRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ii in ent.UserImage
                        where ii.UserID == userID && ii.ID == dr.ID
                        select ii;

            UserImage actImage = query.FirstOrDefault();
            if (actImage != null)
                ObjectHandler.CopyPropertyValues(dr, actImage);

            ent.SaveChanges();
        }

        public void CreateUserImage(DataSetComponent.UserImageDetailRow dr, out int imageID1)
        {
            ISDEntities ent = new ISDEntities();
            UserImageDetail ai = new UserImageDetail();
            ObjectHandler.CopyPropertyValues(dr, ai);
            ent.UserImageDetail.Add(ai);
            ent.SaveChanges();
            imageID1 = ai.ID;
        }

        public void UpdateUserImage(DataSetComponent.UserImageDetailRow dr)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.UserImageDetail
                        where p.ID == dr.ID
                        select p;

            UserImageDetail actImage = query.FirstOrDefault();
            dr.Filename = actImage.Filename;
            dr.isPrimaryImage = Convert.ToBoolean(actImage.isPrimaryImage);

            if (actImage != null)
                ObjectHandler.CopyPropertyValues(dr, actImage);

            ent.SaveChanges();
        }

        public void DeleteUserImage(String UserID, int imageID, out string imageThumbVirtualPath, out string imageVirtualPath)
        {
            ISDEntities ent = new ISDEntities();

            var query = from p in ent.UserImageDetail
                        where p.ID == imageID
                        select p;

            UserImageDetail prodImage = query.FirstOrDefault();
            imageVirtualPath = SystemConstants.UsrImageDirectory + "/" + UserID + "/" + imageID + "_" + prodImage.Filename;
            imageThumbVirtualPath = SystemConstants.UsrImageDirectory + "/" + UserID + "/" + SystemConstants.ImageThumbDirectory + imageID + "_" + prodImage.Filename;
            if (prodImage != null)
                ent.UserImageDetail.Remove(prodImage);

            ent.SaveChanges();
        }

        public DataSetComponent.UserImageRow RetrieveUserImageInformation(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImage
                        where i.UserID == userID
                        select i;
            var ii = query.FirstOrDefault();
            if (ii != null)
            {
                DataSetComponent.UserImageRow dr = new DataSetComponent.UserImageDataTable().NewUserImageRow();
                ObjectHandler.CopyPropertyValues(ii, dr);
                return dr;
            }
            else
                return null;
        }

        public DataSetComponent.UserImageDetailDataTable RetrieveUserImages(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID
                        orderby i.ID
                        select i;

            DataSetComponent.UserImageDetailDataTable dt = new DataSetComponent.UserImageDetailDataTable();
            ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);

            return dt;
        }

        public int RetrieveUserImagesCount(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID
                        orderby i.ID
                        select i;

            return query.Count();
        }

        public DataSetComponent.UserImageDetailRow RetrievePrimaryProductImage(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID && i.isPrimaryImage == true
                        select i;

            DataSetComponent.UserImageDetailRow dr = new DataSetComponent.UserImageDetailDataTable().NewUserImageDetailRow();
            if (query.FirstOrDefault() != null)
            {
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
            }
            return dr;
        }

        public DataSetComponent.UserImageDetailRow RetrieveProductMainImage(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID && i.isPrimaryImage == true
                        select i;

            DataSetComponent.UserImageDetailRow dr = new DataSetComponent.UserImageDetailDataTable().NewUserImageDetailRow();
            UserImageDetail pi = query.FirstOrDefault();
            if (pi != null)
                ObjectHandler.CopyPropertyValues(pi, dr);
            else
            {
                dr.UserID = String.Empty;
                dr.ID = 0;
            }
            return dr;
        }

        public DataSetComponent.UserImageDetailRow RetrieveUserImage(String userID, int imageID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID && i.ID == imageID
                        select i;

            DataSetComponent.UserImageDetailRow dr = new DataSetComponent.UserImageDetailDataTable().NewUserImageDetailRow();
            UserImageDetail ai = query.FirstOrDefault();
            if (ai != null)
                ObjectHandler.CopyPropertyValues(ai, dr);

            return dr;
        }

        public void UpdateUserPrimaryImage(String userID, int imageID)
        {
            ISDEntities ent = new ISDEntities();
            var setMainFalse = from fi in ent.UserImageDetail
                               where fi.isPrimaryImage == true && fi.UserID == userID
                               select fi;

            UserImageDetail pif = setMainFalse.FirstOrDefault();
            if (pif != null)
            {
                pif.isPrimaryImage = false;
                ent.SaveChanges();
            }

            var query = from i in ent.UserImageDetail
                        where i.UserID == userID && i.ID == imageID
                        select i;

            UserImageDetail pit = query.FirstOrDefault();
            if (pit != null)
            {
                pit.isPrimaryImage = true;
                ent.SaveChanges();
            }
        }

        public bool IsUserImageExist(String providerID)
        {
            ISDEntities ent = new ISDEntities();
            var query = from i in ent.UserImage
                        where i.UserID == providerID
                        select i;

            if (query.FirstOrDefault() != null)
            {
                if (query.FirstOrDefault().ImageAmount != 0)
                    return true;
                else return false;
            }
            else return false;

        }

        #endregion

        #region ActImage

        public byte[] RetrieveImageBinary(int imgID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.ActivityImageDetail
                        where e.ID == imgID
                        select e;
            ActivityImageDetail aid = query.FirstOrDefault();

            if (aid != null && aid.ImageStream != null)
            {
                byte[] stream = aid.ImageStream;
                return stream;
            }
            else return null;
        }

        public byte[] RetrieveUserImageBinary(int imgID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.UserImageDetail
                        where e.ID == imgID
                        select e;
            UserImageDetail aid = query.FirstOrDefault();

            if (aid != null && aid.ImageStream != null)
            {
                byte[] stream = aid.ImageStream;
                return stream;
            }
            else return null;
        }

        public byte[] RetrieveAssetBinary(int imgID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.WebAssets
                        where e.ID == imgID
                        select e;
            WebAssets aid = query.FirstOrDefault();

            if (aid != null && aid.FileStream != null)
            {
                byte[] stream = aid.FileStream;
                return stream;
            }
            else return null;
        }

        public DataSetComponent.ActivityImageDetailRow RetrieveImageDetail(int imgID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from e in ent.ActivityImageDetail
                        where e.ID == imgID
                        select e;

            if (query.FirstOrDefault() != null)
            {
                var dr = new DataSetComponent.ActivityImageDetailDataTable().NewActivityImageDetailRow();
                ObjectHandler.CopyPropertyValues(query.FirstOrDefault(), dr);
                return dr;
            }
            else return null;
        }

        #endregion

        #region webconfig

        public DataSetComponent.WebConfigurationRow RetrieveWebConfiguration()
        {
            ISDEntities ent = new ISDEntities();

            var query = from w in ent.WebConfiguration
                        select w;

            WebConfiguration web = query.FirstOrDefault();
            if (web != null)
            {
                var dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
                ObjectHandler.CopyPropertyValues(web, dr);
                return dr;
            }
            else return null;
        }

        public DataSetComponent.WebConfigurationRow RetrieveEmailServerSetting()
        {
            ISDEntities ent = new ISDEntities();

            var query = from w in ent.WebConfiguration
                        select w;

            WebConfiguration web = query.FirstOrDefault();
            if (web != null)
            {
                DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
                ObjectHandler.CopyPropertyValues(web, dr);

                if (dr.IsSMTPAccountNull())
                    return null;
                else return dr;
            }
            else return null;
        }

        #endregion

        #region savedList

        public void RemoveFromSavedList(string username, String userID, int actID)
        {
            string strString = userID.ToString();
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == strString && a.ListValue == actID
                        select a;

            UserSavedList list = query.FirstOrDefault();
            if (list != null)
            {
                ent.UserSavedList.Remove(list);
                ent.SaveChanges();
            }

        }

        public void AddToSavedList(DataSetComponent.UserSavedListRow list)
        {
            ISDEntities ent = new ISDEntities();
            UserSavedList savedList = new UserSavedList();
            ObjectHandler.CopyPropertyValues(list, savedList);
            ent.UserSavedList.Add(savedList);
            ent.SaveChanges();
        }

        public DataSetComponent.UserSavedListDataTable retrieveUserSavedList(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == userID
                        select a;

            var list = query.AsEnumerable();
            if (list != null)
            {
                var dt = new DataSetComponent.UserSavedListDataTable();
                ObjectHandler.CopyEnumerableToDataTable(list, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int retrieveUserSavedListCount(String userID)
        {
            var strID = userID.ToString();
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == strID
                        select a;

            return query.Count();
        }

        public DataSetComponent.UserSavedListDataTable retrieveUserActivityList(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == userID && a.ListType == (int)SystemConstants.SavedListType.Activity
                        select a;

            var list = query.AsEnumerable();
            if (list != null)
            {
                var dt = new DataSetComponent.UserSavedListDataTable();
                ObjectHandler.CopyEnumerableToDataTable(list, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int retrieveUserActivityListCount(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == userID && a.ListType == (int)SystemConstants.SavedListType.Activity
                        select a;

            return query.Count();
        }

        public void InsertNewRewardUser(DataSetComponent.UserRewardRow drr)
        {
            ISDEntities ent = new ISDEntities();
            UserReward user = new UserReward();
            ObjectHandler.CopyPropertyValues(drr, user);

            ent.UserReward.Add(user);
            ent.SaveChanges();
        }

        public DataSetComponent.UserSavedListDataTable retrieveUserRewardList(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == userID && a.ListType == (int)SystemConstants.SavedListType.Reward
                        select a;

            var list = query.AsEnumerable();
            if (list != null)
            {
                var dt = new DataSetComponent.UserSavedListDataTable();
                ObjectHandler.CopyEnumerableToDataTable(list, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public int retrieveUserRewardListCount(String userID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.UserSavedList
                        where a.OwnerGuid == userID && a.ListType == (int)SystemConstants.SavedListType.Reward
                        select a;

            return query.Count();
        }

        #endregion

        #region RewardListing

        public DataSetComponent.v_RewardExplorerDataTable RetrieveRewardCart(string RewardID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query = null;

            //START FILTERING REWARDS BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardID) && RewardID != "0")
            {

                string[] rewards = RewardID.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                query = from a in ent.v_RewardExplorer
                        from s in rewardsInt
                        where a.ID == s
                        select a;
            }
            IEnumerable<v_RewardExplorer> allrewards = query.AsEnumerable();
            if (allrewards != null)
            {
                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;
        }

        public void InsertNewVoucherDetail(DataSetComponent.VoucherDetailsRow dr)
        {
            ISDEntities ent = new ISDEntities();
            VoucherDetails user = new VoucherDetails();
            ObjectHandler.CopyPropertyValues(dr, user);

            ent.VoucherDetails.Add(user);
            ent.SaveChanges();
        }


        public int RetrieveRewardCartCount(string RewardID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query = null;

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardID) && RewardID != "0")
            {

                string[] rewards = RewardID.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                query = from a in ent.v_RewardExplorer
                        from s in rewardsInt
                        where a.ID == s
                        select a;
            }
            IEnumerable<v_RewardExplorer> allrewards = query.AsEnumerable();
            if (allrewards != null)
            {
                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);
                return dt.Count;
            }
            else
                return 0;
        }


        public DataSetComponent.v_UserAttendanceViewDataTable RetrieveActAttendance(String userID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_UserAttendanceView> query = null;

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (userID != String.Empty)
            {

                query = from a in ent.v_UserAttendanceView
                        where a.UserId == userID
                        select a;
            }
            IEnumerable<v_UserAttendanceView> allUsers = query.AsEnumerable();

            if (allUsers != null)
            {
                allUsers = allUsers.OrderByDescending(row => row.CreatedDateTime);
                DataSetComponent.v_UserAttendanceViewDataTable dt = new DataSetComponent.v_UserAttendanceViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allUsers, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;
        }



        public DataSetComponent.v_VoucherExplorerDataTable RetrieveVouchers(string VoucherID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_VoucherExplorer> query = null;

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(VoucherID) && VoucherID != "0")
            {

                string[] Vouchers = VoucherID.Split('|');
                List<string> Vouchersstring = new List<string>();
                foreach (var Voucher in Vouchers)
                {
                    var Voucherstring = Convert.ToString(Voucher);
                    Vouchersstring.Add(Voucherstring);
                }
                query = from a in ent.v_VoucherExplorer
                        from s in Vouchersstring
                        where a.VoucherCode == s
                        select a;
            }
            var allVouchers = query.AsEnumerable();
            if (allVouchers != null)
            {
                DataSetComponent.v_VoucherExplorerDataTable dt = new DataSetComponent.v_VoucherExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allVouchers, dt, LoadOption.OverwriteChanges);
                return dt;
            }
            else
                return null;
        }


        public int RetrieveVoucherCount(string VoucherID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_VoucherExplorer> query = null;

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(VoucherID) && VoucherID != "0")
            {

                string[] Vouchers = VoucherID.Split('|');
                List<string> Vouchersstring = new List<string>();
                foreach (var Voucher in Vouchers)
                {
                    var Voucherstring = Convert.ToString(Voucher);
                    Vouchersstring.Add(Voucherstring);
                }
                query = from a in ent.v_VoucherExplorer
                        from s in Vouchersstring
                        where a.VoucherCode == s
                        select a;
            }
            IEnumerable<v_VoucherExplorer> allVouchers = query.AsEnumerable();
            if (allVouchers != null)
            {
                DataSetComponent.v_VoucherExplorerDataTable dt = new DataSetComponent.v_VoucherExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allVouchers, dt, LoadOption.OverwriteChanges);
                return dt.Count;
            }
            else
                return 0;
        }




        public int RetrieveActAttCount(String userID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_UserAttendanceView> query = null;

            //START FILTERING ACTIVITY BY UserTYPE
            if (userID != String.Empty)
            {


                query = from a in ent.v_UserAttendanceView
                        where a.UserId == userID
                        select a;
            }
            IEnumerable<v_UserAttendanceView> allUsers = query.AsEnumerable();
            if (allUsers != null)
            {
                DataSetComponent.v_UserAttendanceViewDataTable dt = new DataSetComponent.v_UserAttendanceViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allUsers, dt, LoadOption.OverwriteChanges);
                return dt.Count;
            }
            else
                return 0;
        }

        public String getSponsorID(int rewardid)
        {
            ISDEntities ent = new ISDEntities();
            var query = from a in ent.v_RewardExplorer
                        where a.RewardID == rewardid
                        select a;
            if (query.FirstOrDefault() != null)
                return query.FirstOrDefault().ProviderID;
            else return String.Empty;

        }

        public DataSetComponent.v_RewardExplorerDataTable RetrieveAdminRewardsbySearchPhrase(String providerID, string searchKey, int ageFrom, int ageTo, string RewardType, int categoryID, int startIndex, int amount, string sortExpression)
        {

            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query;
            //filtering ProviderID  
            if (providerID != String.Empty)
            {
                query = from a in ent.v_RewardExplorer
                        where a.ProviderID == providerID
                        orderby a.ID
                        select a;
            }
            else
            {
                query = from a in ent.v_RewardExplorer
                        orderby a.ID
                        select a;
            }

            IEnumerable<v_RewardExplorer> Suggestions = (from a in query
                                                         from w in Keywords
                                                         where a.RewardsName.ToLower().Contains(w.ToLower()) || a.RewardDescription.ToLower().Contains(w.ToLower())
                                                         || a.Keywords.ToLower().Contains(w.ToLower())
                                                         select a).Distinct().ToArray();


            //START FILTERING ACTIVITY BY CATEGORY
            if (categoryID != 0)
            {
                Suggestions = from a in Suggestions
                              where a.CategoryID == categoryID
                              select a;
            }


            //START FILTERING REWARDS BY POINTS
            if (ageFrom != 0 && ageTo != 0)
            {
                Suggestions = from a in Suggestions
                              where a.RequiredRewardPoint >= ageFrom && a.RequiredRewardPoint <= ageTo
                              select a;
            }

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardType) && RewardType != "0")
            {

                string[] rewards = RewardType.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                Suggestions = from a in Suggestions
                              from s in rewardsInt
                              where a.RewardSource == s
                              select a;
            }

            IEnumerable<v_RewardExplorer> allrewards = Suggestions.Skip(startIndex).Take(amount).AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                allrewards = allrewards.OrderBy(row => row.RewardsName);

            else if (sortExpression == SystemConstants.sortExpiry)
                allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);

            else if (sortExpression == SystemConstants.sortNameDesc)
                allrewards = allrewards.OrderByDescending(row => row.RewardsName);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
            else if (sortExpression == SystemConstants.sortPoints)
                allrewards = allrewards.OrderBy(row => row.RequiredRewardPoint);
            else if (sortExpression == SystemConstants.sortPointsDesc)
                allrewards = allrewards.OrderByDescending(row => row.RequiredRewardPoint);
            else if (sortExpression == SystemConstants.sortType)
                allrewards = allrewards.OrderByDescending(row => row.RewardType);

            DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);


            return dt;
        }

        public DataSetComponent.v_RewardExplorerDataTable RetrieveAdminRewardsbySearchPhrase(String providerID, string searchKey, string sortExpression)
        {
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query;
            //filtering ProviderID  
            if (providerID != String.Empty)
            {
                query = from a in ent.v_RewardExplorer
                        where a.ProviderID == providerID
                        orderby a.ID
                        select a;
            }
            else
            {
                query = from a in ent.v_RewardExplorer
                        orderby a.ID
                        select a;
            }


            var Suggestions = (from a in query
                               from w in Keywords
                               where a.RewardsName.ToLower().Contains(w.ToLower()) || a.RewardDescription.ToLower().Contains(w.ToLower())
                               || a.Keywords.ToLower().Contains(w.ToLower())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_RewardExplorer> allrewards = Suggestions.AsEnumerable();
            if (allrewards != null)
            {

                if (sortExpression == SystemConstants.sortName)
                    allrewards = allrewards.OrderBy(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatest)
                    allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiry)
                    allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);


                else if (sortExpression == SystemConstants.sortNameDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatestDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardType);
                else if (sortExpression == SystemConstants.sortPoints)
                    allrewards = allrewards.OrderBy(row => row.RequiredRewardPoint);
                else if (sortExpression == SystemConstants.sortPointsDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RequiredRewardPoint);

                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else return null;
        }


        public int RetrieveAdminRewardsbySearchPhraseCount(String providerID, int ageFrom, int ageTo, string RewardType, int categoryID, string searchKey)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardExplorer> query;
            //filtering ProviderID  
            if (providerID != String.Empty)
            {
                query = from a in ent.v_RewardExplorer
                        where a.ProviderID == providerID
                        orderby a.ID
                        select a;
            }
            else
            {
                query = from a in ent.v_RewardExplorer
                        orderby a.ID
                        select a;
            }

            IEnumerable<v_RewardExplorer> Suggestions = (from a in query
                                                         from w in Keywords
                                                         where a.RewardsName.ToLower().Contains(w.ToLower()) || a.RewardDescription.ToLower().Contains(w.ToLower())
                                                         || a.Keywords.ToLower().Contains(w.ToLower())
                                                         select a).Distinct().ToArray();


            //START FILTERING ACTIVITY BY CATEGORY
            if (categoryID != 0)
            {
                Suggestions = from a in Suggestions
                              where a.CategoryID == categoryID
                              select a;
            }


            //START FILTERING REWARDS BY POINTS
            if (ageFrom != 0 && ageTo != 0)
            {
                Suggestions = from a in Suggestions
                              where a.RequiredRewardPoint >= ageFrom && a.RequiredRewardPoint <= ageTo
                              select a;
            }

            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardType) && RewardType != "0")
            {

                string[] rewards = RewardType.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                Suggestions = from a in Suggestions
                              from s in rewardsInt
                              where a.RewardSource == s
                              select a;
            }

            IEnumerable<v_RewardExplorer> activities = Suggestions.AsEnumerable();
            DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return Suggestions.Count();
        }


        public DataSetComponent.v_RewardExplorerDataTable RetrieveAdminRewards(int categoryID, String providerID, int ageFrom, int ageTo, string RewardType, string sortExpression, int startIndex, int amount)
        {

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_RewardExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_RewardExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID))
                        orderby sortExpression
                        select a;
            }

            else
            {
                query = from a in ent.v_RewardExplorer

                        orderby sortExpression
                        select a;
            }

            //START FILTERING ACTIVITY BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                query = from a in query
                        where a.RequiredRewardPoint >= ageFrom && a.RequiredRewardPoint <= ageTo
                        orderby sortExpression
                        select a;
            }



            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardType) && RewardType != "0")
            {

                string[] rewards = RewardType.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                query = from a in query
                        from s in rewardsInt
                        where a.RewardSource == s
                        select a;
            }
            IEnumerable<v_RewardExplorer> allrewards = query.AsEnumerable();

            if (allrewards != null)
            {

                if (sortExpression == SystemConstants.sortName)
                    allrewards = allrewards.OrderBy(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatest)
                    allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiry)
                    allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortPoints)
                    allrewards = allrewards.OrderBy(row => row.RequiredRewardPoint);
                else if (sortExpression == SystemConstants.sortPointsDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RequiredRewardPoint);

                else if (sortExpression == SystemConstants.sortNameDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatestDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);

                allrewards = allrewards.Skip(startIndex).Take(amount);

                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else return null;
        }

        public DataSetComponent.v_RewardExplorerDataTable RetrieveAdminRewards(String providerID, int categoryID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_RewardExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_RewardExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID))
                        orderby sortExpression
                        select a;
            }

            else
            {
                query = from a in ent.v_RewardExplorer
                        orderby sortExpression
                        select a;
            }

            IEnumerable<v_RewardExplorer> allrewards = query.AsEnumerable();
            if (allrewards != null)
            {
                if (sortExpression == SystemConstants.sortName)
                    allrewards = allrewards.OrderBy(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatest)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiry)
                    allrewards = allrewards.OrderBy(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortNameDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardsName);
                else if (sortExpression == SystemConstants.sortLatestDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortExpiryDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RewardExpiryDate);
                else if (sortExpression == SystemConstants.sortPoints)
                    allrewards = allrewards.OrderBy(row => row.RequiredRewardPoint);
                else if (sortExpression == SystemConstants.sortPointsDesc)
                    allrewards = allrewards.OrderByDescending(row => row.RequiredRewardPoint);
                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(allrewards, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else return null;

        }

        public int RetrieveAdminRewardsCount(String providerID, int ageFrom, int ageTo, string RewardType, int categoryID)
        {

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            // GET ALL activity in category
            IQueryable<v_RewardExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_RewardExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID))
                        select a;
            }

            else
            {
                query = from a in ent.v_RewardExplorer

                        select a;
            }


            //START FILTERING Reward BY AGE
            if (ageFrom != 0 && ageTo != 0)
            {
                query = from a in query
                        where a.RequiredRewardPoint >= ageFrom && a.RequiredRewardPoint <= ageTo
                        select a;
            }
            //START FILTERING ACTIVITY BY REWARDTYPE
            if (!string.IsNullOrEmpty(RewardType) && RewardType != "0")
            {

                string[] rewards = RewardType.Split('|');
                List<int> rewardsInt = new List<int>();
                foreach (var reward in rewards)
                {
                    var rewardInt = Convert.ToInt32(reward);
                    rewardsInt.Add(rewardInt);
                }
                query = from a in query
                        from s in rewardsInt
                        where a.RewardSource == s
                        select a;
            }


            IEnumerable<v_RewardExplorer> activities = query.AsEnumerable();

            if (activities != null)
            {
                DataSetComponent.v_RewardExplorerDataTable dt = new DataSetComponent.v_RewardExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);


                return query.Count();
            }
            else return 0;
        }

        public DataSetComponent.v_RewardUserExplorerRow RetrieveUserRewardDetails(string Uname)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_RewardUserExplorer> query = null;
            query = from i in ent.v_RewardUserExplorer
                    where i.FirstName == Uname
                    select i;
            if (query.Count() == 0)
            {
                return null;
            }
            var Reward = query.FirstOrDefault();
            if (Reward != null)
            {
                DataSetComponent.v_RewardUserExplorerRow dr = new DataSetComponent.v_RewardUserExplorerDataTable().Newv_RewardUserExplorerRow();
                ObjectHandler.CopyPropertyValues(Reward, dr);
                return dr;
            }
            else

                return null;


        }

        #endregion

        #region Visitor
        public void InsertVisitor(DataSetComponent.ActivityVisitorRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ActivityVisitor vis = new ActivityVisitor();
            ObjectHandler.CopyPropertyValues(dr, vis);
            ent.ActivityVisitor.Add(vis);
            ent.SaveChanges();
        }
        #endregion

        public void InsertVisitors(DataSetComponent.ActivityVisitorDataTable dt)
        {
            ISDEntities ent = new ISDEntities();
            foreach (var dr in dt)
            {

                ActivityVisitor vis = new ActivityVisitor();
                ObjectHandler.CopyPropertyValues(dr, vis);
                ent.ActivityVisitor.Add(vis);
            }
            ent.SaveChanges();
        }

        public HashSet<int> RetrieveActivitiesIDs()
        {
            ISDEntities ent = new ISDEntities();

            return new HashSet<int>(ent.Activity.Select(y => y.ID));
        }









        #region Keyword

        public bool CheckAdvanceSearch()
        {
            ISDEntities ent = new ISDEntities();
            DataSetComponent.WebConfigurationDataTable dt = new DataSetComponent.WebConfigurationDataTable();
            DataSetComponent.WebConfigurationRow dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
            var query = from w in ent.WebConfiguration
                        select w;

            if (query.FirstOrDefault() != null)
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.OverwriteChanges);
                foreach (var drow in dt)
                {
                    dr.AdvancedSearch = drow.AdvancedSearch;
                }
            }
            return dr.AdvancedSearch;
        }

        public DataSetComponent.v_KeyCollectionViewDataTable SearchKeywordCollection(String searchKey)
        {
            String[] Keywords = Array.ConvertAll(searchKey.Split(' '), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            var dt = new DataSetComponent.v_KeyCollectionViewDataTable();

            var query = (from a in ent.Keyword
                         from w in Keywords
                         where a.Keywords.ToLower().Contains(w.ToLower())
                         select a).Distinct();

            if (query.AsEnumerable() != null)
            {
                ObjectHandler.CopyEnumerableToDataTable(query, dt, LoadOption.OverwriteChanges);
            }
            else
                return null;
            return dt;
        }
        #endregion

        #region ActivityListing
        /*ActivityView contains attributes used in reports only, ActivityExplorer contains complete 
         *attributes used in activity listing and details. use activity ID to retrieve the activities
         *you want to be show. int[] ActivityID. */

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivitiesbySearchPhrase(String providerID, string searchKey, int startIndex, int amount, string sortExpression)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query = from a in ent.v_ActivityExplorer
                                                   where a.ProviderID == providerID
                                                   orderby a.ID
                                                   select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                               || a.Keywords.ToLower().Contains(w.ToLower())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_ActivityExplorer> activities = Suggestions.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);

            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);

            activities = activities.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
            dt.OrderBy(row => row.Name);
            return dt;
        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivitiesbySearchPhrase(String providerID, string searchKey, string sortExpression)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query = from a in ent.v_ActivityExplorer
                                                   where a.ProviderID == providerID
                                                   orderby a.ID
                                                   select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                               || a.Keywords.ToLower().Contains(w.ToLower())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_ActivityExplorer> activities = Suggestions.AsEnumerable();
            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);

            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);
            DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesReportbySearchPhrase(String providerID, string searchKey, string sortExpression)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityView> query = from a in ent.v_ActivityView
                                               where a.ProviderID == providerID
                                               orderby a.ID
                                               select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                               || a.Keywords.ToLower().Contains(w.ToLower())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_ActivityView> activities = Suggestions.AsEnumerable();
            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveProviderActivitiesbySearchPhraseCount(String providerID, string searchKey)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query = from a in ent.v_ActivityExplorer
                                                   where a.ProviderID == providerID
                                                   orderby a.ID
                                                   select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                               || a.Keywords.ToLower().Contains(w.ToLower())
                               select a).Distinct().ToArray();

            return Suggestions.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesReport(String providerID, int categoryID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityView> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            IEnumerable<v_ActivityView> activities = query.AsEnumerable();
            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityModifiedDateTime);
            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);
            dt.OrderBy(row => row.Name);
            return dt;
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveProviderActivitiesFilteredReport(String ProviderID, string SearchKey, int ageFrom, int ageTo, int postCode, int CategoryID, int startIndex, int amount, string sortExpression)
        {

            ISDEntities ent = new ISDEntities();
            IEnumerable<v_ActivityView> activitiesReport;

            IQueryable<v_ActivityView> providerCatFiltered;
            IQueryable<v_ActivityView> paramFiltered;
            if (CategoryID == 0)
                providerCatFiltered = from a in ent.v_ActivityView
                                      where a.ProviderID == ProviderID
                                      orderby a.ID
                                      select a;
            else
                providerCatFiltered = from a in ent.v_ActivityView
                                      where (a.ProviderID == ProviderID && (
                                                      a.CategoryID == CategoryID ||
                                                      a.CategoryLevel1ParentID == CategoryID ||
                                                      a.CategoryLevel2ParentID == CategoryID))
                                      select a;


            paramFiltered = from a in providerCatFiltered
                            where a.AgeFrom >= ageFrom && a.AgeTo <= ageTo
                            select a;

            if (postCode != 0)
                paramFiltered = from a in providerCatFiltered
                                where a.SuburbID == postCode
                                select a;

            if (!string.IsNullOrEmpty(SearchKey))
            {
                String[] Keywords = Array.ConvertAll(SearchKey.Split(';'), c => c.Trim());

                var querySearchFiltered = (from a in paramFiltered
                                           from w in Keywords
                                           where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                                           || a.Keywords.ToLower().Contains(w.ToLower())
                                           select a).Distinct().ToArray();
                activitiesReport = querySearchFiltered.AsEnumerable();
            }
            else
            {
                activitiesReport = paramFiltered.AsEnumerable();
            }

            var dt = new DataSetComponent.v_ActivityViewDataTable();
            if (activitiesReport != null)
            {
                ObjectHandler.CopyEnumerableToDataTable(activitiesReport, dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;
        }

        public int RetrieveProviderActivitiesFilteredReportCount(String ProviderID, string SearchKey, int ageFrom, int ageTo, int postCode, int CategoryID)
        {

            ISDEntities ent = new ISDEntities();
            IEnumerable<v_ActivityView> activitiesReport;

            IQueryable<v_ActivityView> providerCatFiltered;
            IQueryable<v_ActivityView> paramFiltered;
            if (CategoryID == 0)
                providerCatFiltered = from a in ent.v_ActivityView
                                      where a.ProviderID == ProviderID
                                      orderby a.ID
                                      select a;
            else
                providerCatFiltered = from a in ent.v_ActivityView
                                      where (a.ProviderID == ProviderID && (
                                                      a.CategoryID == CategoryID ||
                                                      a.CategoryLevel1ParentID == CategoryID ||
                                                      a.CategoryLevel2ParentID == CategoryID))
                                      select a;


            paramFiltered = from a in providerCatFiltered
                            where a.AgeFrom >= ageFrom && a.AgeTo <= ageTo
                            select a;

            if (postCode != 0)
                paramFiltered = from a in providerCatFiltered
                                where a.SuburbID == postCode
                                select a;

            if (!string.IsNullOrEmpty(SearchKey))
            {
                String[] Keywords = Array.ConvertAll(SearchKey.Split(';'), c => c.Trim());

                var querySearchFiltered = (from a in paramFiltered
                                           from w in Keywords
                                           where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                                           || a.Keywords.ToLower().Contains(w.ToLower())
                                           select a).Distinct().ToArray();
                activitiesReport = querySearchFiltered.AsEnumerable();
            }
            else
            {
                activitiesReport = paramFiltered.AsEnumerable();
            }

            var dt = new DataSetComponent.v_ActivityViewDataTable();
            if (activitiesReport != null)
            {

                return activitiesReport.Count();
            }
            else
                return 0;
        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivities(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }



            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);

            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);

            activities = activities.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivities(String providerID, int categoryID, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }



            IEnumerable<v_ActivityExplorer> activities = query.AsEnumerable();

            if (sortExpression == SystemConstants.sortName)
                activities = activities.OrderBy(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatest)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiry)
                activities = activities.OrderBy(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPrice)
                activities = activities.OrderByDescending(row => row.IsPaid);

            else if (sortExpression == SystemConstants.sortNameDesc)
                activities = activities.OrderByDescending(row => row.Name);
            else if (sortExpression == SystemConstants.sortLatestDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ModifiedDateTime);
            else if (sortExpression == SystemConstants.sortPriceDesc)
                activities = activities.OrderBy(row => row.IsPaid);
            else if (sortExpression == SystemConstants.sortExpiryDesc)
                activities = activities.OrderByDescending(row => row.ActivityType);

            DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveProviderActivitiesCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityExplorer> query;

            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID
                        select a;
            }

            return query.Count();
        }

        public DataSetComponent.v_ActivityExplorerDataTable RetrieveProviderActivitiesbyCategoryID(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.v_ActivityExplorerDataTable dt = new DataSetComponent.v_ActivityExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveProviderActivitiesbyCategoryIDCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityExplorer> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityExplorer
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityExplorer
                        where a.ProviderID == providerID
                        select a;
            }
            return query.Count();
        }



        /*
        public DataSetComponent.v_ActivityViewDataTable RetrieveSearchActivities(string searchKey, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.v_ActivityView
                        where a.Name.Contains(searchKey) || a.CategoryName.Contains(searchKey)
                        || a.Name.Contains(searchKey) || a.CategoryLevel1ParentName.Contains(searchKey)
                        || a.CategoryLevel2ParentName.Contains(searchKey)
                        orderby sortExpression
                        select a;

            IEnumerable<v_ActivityView> activities = query.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveSearchActivitiesCount(string searchKey)
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.v_ActivityView
                        where a.Name.Contains(searchKey) || a.CategoryName.Contains(searchKey)
                        || a.Name.Contains(searchKey) || a.CategoryLevel1ParentName.Contains(searchKey)
                        || a.CategoryLevel2ParentName.Contains(searchKey)
                        select a;

            return query.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveSearchProviderActivities(String providerID, String searchKey, int startIndex, int amount, string sortExpression)
        {
            //splitting keywords as keyword can be a multiple words
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ProviderID  
            IQueryable<v_ActivityView> query = from a in ent.v_ActivityView
                                               where a.ProviderID == providerID
                                               orderby a.ID
                                               select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.ToLower().Contains(w.ToLower()) || a.ShortDescription.ToLower().Contains(w.ToLower())
                               orderby sortExpression
                               select a).Distinct().ToArray();

            IEnumerable<v_ActivityView> activities = Suggestions.Skip(startIndex).Take(amount).AsEnumerable();

            DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
            ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

            return dt;
        }

        public int RetrieveSearchProviderActivitiesCount(String providerID, string searchKey)
        {
            String[] Keywords = Array.ConvertAll(searchKey.Split(';'), c => c.Trim());

            ISDEntities ent = new ISDEntities();

            //filtering ID  
            IQueryable<v_ActivityView> query = from a in ent.v_ActivityView
                                               where a.ProviderID == providerID
                                               select a;

            var Suggestions = (from a in query
                               from w in Keywords
                               where a.Name.Contains(w)
                               select a).Distinct().ToArray();

            return Suggestions.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveActivityViews(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivityViewsCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        select a;
            }
            return query.Count();
        }

        public DataSetComponent.v_ActivityViewDataTable RetrieveActivityViewsFromActivitiesIDArray(String providerID, int[] activitiesID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.ProviderID == providerID && (
                                        a.CategoryID == categoryID ||
                                        a.CategoryLevel1ParentID == categoryID ||
                                        a.CategoryLevel2ParentID == categoryID))
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.v_ActivityView
                        where (a.CategoryID == categoryID ||
                                   a.CategoryLevel1ParentID == categoryID ||
                                   a.CategoryLevel2ParentID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.v_ActivityViewDataTable dt = new DataSetComponent.v_ActivityViewDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivityViewsFromActivitiesIDArrayCount(String providerID, int[] activitiesID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<v_ActivityView> query = null;
            if (providerID != String.Empty)
            {
                query = from a in ent.v_ActivityView
                        where a.ProviderID == providerID
                        select a;
            }
            else
            {
                query = (from a in query
                         from w in activitiesID
                         where a.ID.ToString().Contains(w.ToString())
                         select a).Distinct().ToArray();
            }

            return query.Count();
        }

        public DataSetComponent.ActivityDataTable RetrieveActivities(String providerID, int categoryID, int startIndex, int amount, string sortExpression)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<Activity> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.ProviderID == providerID &&
                                        a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.Activity
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }

            var activities = query.Skip(startIndex).Take(amount).AsEnumerable();
            if (activities != null)
            {
                DataSetComponent.ActivityDataTable dt = new DataSetComponent.ActivityDataTable();
                ObjectHandler.CopyEnumerableToDataTable(activities, dt, LoadOption.OverwriteChanges);

                return dt;
            }
            else
                return null;
        }

        public int RetrieveActivitiesCount(String providerID, int categoryID)
        {
            ISDEntities ent = new ISDEntities();
            IQueryable<Activity> query = null;
            if (providerID != String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.ProviderID == providerID &&
                                        a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else if (providerID == String.Empty && categoryID != 0)
            {
                query = from a in ent.Activity
                        where (a.CategoryID == categoryID)
                        orderby sortExpression
                        select a;
            }
            else
            {
                query = from a in ent.Activity
                        where a.ProviderID == providerID
                        orderby sortExpression
                        select a;
            }
            return query.Count();
        }
        */
        #endregion











        #region Users

        public DataSetComponent.v_UserExplorerDataTable RetrieveUserExplorer()
        {
            ISDEntities ent = new ISDEntities();

            var query = from u in ent.v_UserExplorer
                        select u;

            if (query != null)
            {
                var dt = new DataSetComponent.v_UserExplorerDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        #region Provider

        public void InsertNewProviderProfiles(DataSetComponent.ProviderProfilesRow dr)
        {
            ISDEntities ent = new ISDEntities();
            ProviderProfiles prov = new ProviderProfiles();
            ObjectHandler.CopyPropertyValues(dr, prov);

            ent.ProviderProfiles.Add(prov);
            ent.SaveChanges();
        }


        #endregion

        #endregion

        public bool ProviderNameExist(string organisationName)
        {
            ISDEntities ent = new ISDEntities();

            var query = from o in ent.ProviderProfiles
                        where o.ProviderName == organisationName
                        select o;

            if (query.FirstOrDefault() != null)
                return true;
            else return false;
        }

        public void UpdateUserImageInformation(DataSetComponent.UserImageRow usrImagDet)
        {
            ISDEntities ent = new ISDEntities();

            var query = from ac in ent.UserImage
                        where ac.ID == usrImagDet.ID
                        select ac;

            UserImage provider = query.FirstOrDefault();
            if (provider != null)
            {
                ObjectHandler.CopyPropertyValues(usrImagDet, provider);
                ent.SaveChanges();
            }
        }


        public void ChangeActivityEmailAddress(int activityID, string newEmailAddress)
        {
            ISDEntities ent = new ISDEntities();
            var query = from a in ent.ActivityContactDetail
                        where a.ActivityID == activityID
                        select a;

            if (query.Count() != 0)
            {
                ActivityContactDetail activities = query.FirstOrDefault();

                if (activities != null)
                {
                    activities.Email = newEmailAddress;
                    ent.SaveChanges();
                }
            }
        }

        public DataSetComponent.ActivityReferenceCodeDataTable RetrieveActivityReference()
        {
            ISDEntities ent = new ISDEntities();

            var query = from a in ent.ActivityReferenceCode
                        select a;

            if (query.AsEnumerable() != null)
            {
                var dt = new DataSetComponent.ActivityReferenceCodeDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else
                return null;
        }

        #region VisitorStat

        public int RetrieveProviderVisitorCount(String ProviderID, DateTime From, DateTime To)
        {
            ISDEntities ent = new ISDEntities();

            HashSet<int> providerActs = new HashSet<int>(ent.Activity.Where(x => x.ProviderID == ProviderID).Select(y => y.ID));
            return ent.ActivityVisitor.Where(x => providerActs.Contains(x.ActivityID.Value) && x.CreatedDatetime >= From && x.CreatedDatetime <= To).Count();
        }

        public int RetrieveActivityVisitorCount(int ActivityID, DateTime From, DateTime To)
        {
            ISDEntities ent = new ISDEntities();
            return ent.ActivityVisitor.Where(x => x.ActivityID == ActivityID && x.CreatedDatetime >= From && x.CreatedDatetime <= To).Count();
        }

        public DataSetComponent.ActivityVisitorDataTable RetrieveActivityVisitorData(int ActivityID, DateTime From, DateTime To)
        {
            ISDEntities ent = new ISDEntities();
            var query = ent.ActivityVisitor.Where(x => x.ActivityID == ActivityID && x.CreatedDatetime >= From && x.CreatedDatetime <= To);
            if (query.AsEnumerable() != null)
            {
                DataSetComponent.ActivityVisitorDataTable dt = new DataSetComponent.ActivityVisitorDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        public DataSetComponent.ActivityVisitorDataTable RetrieveProviderVisitorData(String ProviderID, DateTime From, DateTime To)
        {
            ISDEntities ent = new ISDEntities();

            HashSet<int> providerActs = new HashSet<int>(ent.Activity.Where(x => x.ProviderID == ProviderID).Select(y => y.ID));
            var query = ent.ActivityVisitor
                .Where(x => providerActs.Contains(x.ActivityID.Value) && x.CreatedDatetime >= From && x.CreatedDatetime <= To);

            if (query.AsEnumerable() != null)
            {
                DataSetComponent.ActivityVisitorDataTable dt = new DataSetComponent.ActivityVisitorDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;
        }

        #endregion






        public object RetrieveProviderClinicians(string providerID)
        {
            ISDEntities ent = new ISDEntities();

            var query = from c in ent.Clinicians
                        where c.ProviderID == providerID
                        select c;
            if (query.AsEnumerable() != null)
            {
                var dt = new DataSetComponent.CliniciansDataTable();
                ObjectHandler.CopyEnumerableToDataTable(query.AsEnumerable(), dt, LoadOption.PreserveChanges);
                return dt;
            }
            else return null;

        }

        public List<ActivityReferenceCode> RetrieveActivityReferences()
        {
           return  ent.ActivityReferenceCode.ToList();
        }
    }


}

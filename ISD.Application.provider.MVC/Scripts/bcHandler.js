function updateSize() {
  
    for (var nFileId = 0; nFileId < nFiles; nFileId++) {
        nBytes += oFiles[nFileId].size;
    }
    var sOutput = nBytes + " bytes";
    // optional code for multiples approximation
    for (var aMultiples = ["KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"], nMultiple = 0, nApprox = nBytes / 1024; nApprox > 1; nApprox /= 1024, nMultiple++) {
        sOutput = nApprox.toFixed(3) + " " + aMultiples[nMultiple] + " (" + nBytes + " bytes)";
    }
    // end of optional code
    document.getElementById("fileNum").innerHTML = nFiles;
    document.getElementById("fileSize").innerHTML = sOutput;
}

window.URL = window.URL || window.webkitURL;


function handleFiles(files) {

    var nBytes = 0,
      nFiles = files.length;

    if (!files.length) {
        fileList.innerHTML = "<p>No files selected!</p>";
    } else {
        for (var i = 0; i < files.length; i++) {
            nBytes += files[i].size;
            var div = document.createElement("div");
            div.className = "col-md-3 col-sm-6";
            fileList.appendChild(div);

            var span = document.createElement("span");
            span.className = "overlay-zoom";
            div.appendChild(span);

            var img = document.createElement("img");
            img.src = window.URL.createObjectURL(files[i]);
            img.height = 166;
            img.width = 250;
            img.onload = function (e) {
                window.URL.revokeObjectURL(this.src);
            }
            span.appendChild(img);

            var link = document.createElement("a");
            
            link.addEventListener("click", function (e) {
                if (fileElem) {
                    fileElem.click();
                }
                e.preventDefault(); // prevent navigation to "#"
            }, false);

            div.appendChild(spanZoom);

            var spanZoom = document.createElement("span");
            spanZoom.innerHTML = files[i].name + ": " + files[i].size + " bytes";
            div.appendChild(spanZoom);



        }
        var sOutput = nBytes + " bytes";
        // optional code for multiples approximation
        for (var aMultiples = ["KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"], nMultiple = 0, nApprox = nBytes / 1024; nApprox > 1; nApprox /= 1024, nMultiple++) {
            sOutput = nApprox.toFixed(3) + " " + aMultiples[nMultiple] + " (" + nBytes + " bytes)";
        }
        document.getElementById("fileNum").innerHTML = nFiles;
        document.getElementById("fileSize").innerHTML = sOutput;
    }
}



function CheckNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
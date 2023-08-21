// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function uploadFiles() {
    var files = document.getElementById("fileInput").files;
    var previewContainer = document.getElementById("previewContainer");
    
    previewContainer.innerHTML = ""; // Очистить контейнер превью
    
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var reader = new FileReader();
        
        reader.onload = function(e) {
            var img = document.createElement("img");
            img.src = e.target.result;
            previewContainer.appendChild(img);
        };
        
        reader.readAsDataURL(file);
    }
}
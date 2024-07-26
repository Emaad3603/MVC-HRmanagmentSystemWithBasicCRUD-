// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var searchInput =Document.getElementById("SearchInput");
searchInput.addEventListener("Keyup", function ({


    // Creating Our XMLHttpRequest object 
    let xhr = new XMLHttpRequest();

// Making our connection  
let url = 'https://localhost:44377/Employee/Index?SearchInput=${searchInput.Value}';
xhr.open("GET", url, true);

// function execute after request is successful 
xhr.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        console.log(this.responseText);
    }
}
// Sending our request 
xhr.send();
}))
"use strict";
var file = {};
file.includeHTML = function (cb) {
  var elements = document.querySelectorAll("[file-include-html]");
  var elementCount = elements.length;

  if (elementCount === 0 && cb) {
    cb();
    return;
  }

  elements.forEach(function (element) {
    var file = element.getAttribute("file-include-html");
    var xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
      if (this.readyState == 4) {
        if (this.status == 200) {
          element.innerHTML = this.responseText;
        } else if (this.status == 404) {
          element.innerHTML = "Page not found.";
        }

        element.removeAttribute("file-include-html");
        file.includeHTML(cb); // Recursive call outside the event handler
      }
    };

    xhttp.open("GET", file, true);
    xhttp.send();
  });
};

// Example usage:
file.includeHTML(function () {
  // Callback function to be executed after includes are loaded
});
// LuxeStore site.js

document.addEventListener("DOMContentLoaded", function () {

    console.log("LuxeStore JS Loaded");

    // Debug login form submit
    const forms = document.querySelectorAll("form");

    forms.forEach(function (form) {
        form.addEventListener("submit", function () {
            console.log("Form submitted");
        });
    });

});
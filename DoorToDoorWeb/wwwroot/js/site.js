﻿const d2dServerUrl = "https://localhost:44343/";


const formToggleElement = document.getElementById("show-hide-form");
const formElement = document.querySelector("form");
formElement.hidden = !holdForm;

function toggleForm(showMessage, hideMessage) {
    formElement.toggleAttribute("hidden");

    if (formElement.hidden) {
        formToggleElement.innerText = showMessage;
    }
    else {
        formToggleElement.innerText = hideMessage;
    }
}

formToggleElement.addEventListener('click', (event) => {
    toggleForm(showMessage, hideMessage);
});


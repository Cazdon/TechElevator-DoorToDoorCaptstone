const formToggleElement = document.getElementById("show-hide-form");
const formElement = document.querySelector("form");
formElement.hidden = true;

function toggleForm() {
    formElement.toggleAttribute("hidden");

    if (formElement.hidden) {
        formToggleElement.innerText = "Show Register Salesperson Form";
    }
    else {
        formToggleElement.innerText = "Hide Register Salesperson Form";
    }
}

formToggleElement.addEventListener('click', (event) => {
    toggleForm();
});
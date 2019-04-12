const formToggleElement = document.getElementById("show-hide-form");

function toggleForm() {
    let formElement = document.querySelector("form");

    formElement.toggleAttribute("hidden");
}

formToggleElement.addEventListener('click', (event) => {
    toggleForm();
});
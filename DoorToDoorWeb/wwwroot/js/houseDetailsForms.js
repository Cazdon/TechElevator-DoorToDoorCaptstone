const noteShowMessage = "Show Add Note Form";
const noteHideMessage = "Hide Add Note Form";

const noteFormToggleElement = document.getElementById("show-hide-note-form");
const noteFormElement = document.getElementById("add-note-form");
noteFormElement.hidden = !holdNoteForm;

const residentShowMessage = "Show Add Resident Form";
const residentHideMessage = "Hide Add Resident Form";

const residentFormToggleElement = document.getElementById("show-hide-resident-form");
const residentFormElement = document.getElementById("add-resident-form");
residentFormElement.hidden = !holdResidentForm;

function toggleForm(showMessage, hideMessage, form, toggle) {
    form.toggleAttribute("hidden");

    if (form.hidden) {
        toggle.innerText = showMessage;
    }
    else {
        toggle.innerText = hideMessage;
    }
}

noteFormToggleElement.addEventListener('click', (event) => {
    event.stopPropagation();
    toggleForm(noteShowMessage, noteHideMessage, noteFormElement, noteFormToggleElement);
});

residentFormToggleElement.addEventListener('click', (event) => {
    event.stopPropagation();
    toggleForm(residentShowMessage, residentHideMessage, residentFormElement, residentFormToggleElement);
});
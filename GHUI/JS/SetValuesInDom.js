function setValues(elementId, value) {
    const elem = document.getElementById(elementId);

    if (elem === null) return;

    // if it's text
    if (elem.innerText) {
        elem.innerText = value;
    }

    // if it's an input type
    else if (elem.type) {

        // if checkbox or radio, convert to boolean and apply to checked property
        if (elem.type === 'checkbox' || elem.type === 'radio' ) {
            elem.checked = (value.toLowerCase() === 'true');
        }
        // if number based, convert to number and set the value
        else if (elem.type === 'range' || elem.type === 'number') {
            elem.value = Number(value);
        }
        elem.value = value;
    }
}
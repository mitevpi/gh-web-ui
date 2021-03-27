function queryInputElements() {
    const returnObjects = [];
    const inputElements = document.getElementsByTagName('input');

    for (const htmlElement of inputElements) {
        const inputData = new Object();

        inputData.id = htmlElement.id;
        inputData.value = handleValueExtract(htmlElement);
        inputData.name = htmlElement.name;
        inputData.max = htmlElement.max;
        inputData.min = htmlElement.min;
        inputData.isChecked = htmlElement.checked;
        inputData.type = htmlElement.type;
        returnObjects.push(inputData);
    }
    return returnObjects;
}

function handleValueExtract(htmlElement) {
    if (htmlElement.type === 'checkbox') {
        return htmlElement.checked;
    } else if (htmlElement.type === 'radio') {
        return htmlElement.checked;
    } else {
        return htmlElement.value;
    }
}

//queryInputElements();
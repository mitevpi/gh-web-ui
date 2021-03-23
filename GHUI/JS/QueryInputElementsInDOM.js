function queryInputElements() {
    const returnObjects = [];
    const inputElements = document.getElementsByTagName('input');

    for (const c of inputElements) {
        const inputData = new Object();

        inputData.id = c.id;
        inputData.value = c.value;
        inputData.name = c.name;
        inputData.max = c.max;
        inputData.min = c.min;
        inputData.isChecked = c.checked;
        inputData.type = c.type;
        returnObjects.push(inputData);
    }
    return returnObjects;
}
queryInputElements();
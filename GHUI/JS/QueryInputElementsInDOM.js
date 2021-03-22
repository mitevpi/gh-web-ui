function queryInputElements() {
    const returnObjects = [];
    const inputElements = document.getElementsByTagName('input');

    for (const c of inputElements) {
        const inputData = new Object();
        inputData.id = c.id;
        inputData.value = c.value;
        returnObjects.push(inputData);
    }
    return returnObjects;
}
queryInputElements();
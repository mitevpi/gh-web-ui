document.addEventListener('click', function (event) {
    const elem = event.target;
    const returnObject =
    {
        type: 'click',
        targetId: elem.id,
        targetName: elem.name,
        targetType: elem.type,
        targetValue: elem.value,
        targetTagName: elem.tagName
    };
    //console.log(returnObject);
    window.chrome.webview.postMessage(returnObject);
});

document.addEventListener('keydown', function (event) {
    const returnObject =
    {
        type: 'keydown'
    };
    window.chrome.webview.postMessage(returnObject);
});

document.addEventListener('keypress', function (event) {
    const returnObject =
    {
        type: 'keydown'
    };
    window.chrome.webview.postMessage(returnObject);
});
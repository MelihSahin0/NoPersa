function isOverflowActive(elementId) {
    var element = document.getElementById(elementId);
    return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
}
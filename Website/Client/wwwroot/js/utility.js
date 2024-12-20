function isOverflowActive(elementId) {
    var element = document.getElementById(elementId);
    return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
}

function downloadFile(filename, data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    const link = document.createElement('a'); 
    const url = URL.createObjectURL(blob);
    link.href = url;  
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url)
};

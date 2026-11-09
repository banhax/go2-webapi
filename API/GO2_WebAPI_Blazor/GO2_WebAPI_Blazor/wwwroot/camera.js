window.startVideo = async (elementId) => {
    const video = document.getElementById(elementId);
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        video.play();
    } catch (err) {
        console.error("Error accessing camera: ", err);
    }
};

window.stopVideo = (elementId) => {
    const video = document.getElementById(elementId);
    const stream = video.srcObject;
    if (stream) {
        const tracks = stream.getTracks();
        tracks.forEach(track => track.stop());
        video.srcObject = null;
    }
};
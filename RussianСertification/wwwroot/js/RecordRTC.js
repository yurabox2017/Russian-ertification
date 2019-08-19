// PostBlob method uses XHR2 and FormData to submit 
// recorded blob to the PHP server
function PostBlob(blob) {
    // FormData
    var formData = new FormData();
    //fileName = (Math.round(Math.random() * 99999999) + 99999999) + '.webm';
    //var b = new Blob(['hello world']);
    formData.append('formFile', fileName);
    formData.append('formFile', blob);
    //formData.append('video-filename', fileName);
    //formData.append('video-blob', blob);

    // progress-bar
    var hr = document.createElement('hr');
    container.appendChild(hr);
    var strong = document.createElement('strong');
    strong.id = 'percentage';
    strong.innerHTML = 'Video upload progress: ';
    container.appendChild(strong);
    var progress = document.createElement('progress');
    container.appendChild(progress);

    // POST the Blob using XHR2
    xhr('/RecordRTC/PostRecordedAudioVideo', formData, progress, percentage, function (fName) {
        container.appendChild(document.createElement('hr'));
        var mediaElement = document.createElement('video');

        var source = document.createElement('source');
        source.src = document.location.origin + '/uploads/' + fName.replace(/"/g, '');
        source.type = 'video/webm; codecs="vp8, vorbis"';

        mediaElement.appendChild(source);

        mediaElement.controls = true;
        container.appendChild(mediaElement);
        mediaElement.play();

        progress.parentNode.removeChild(progress);
        strong.parentNode.removeChild(strong);
        hr.parentNode.removeChild(hr);
    });
}

var record = document.getElementById('record');
var stop = document.getElementById('stop');
var deleteFiles = document.getElementById('delete');

var preview = document.getElementById('preview');

var container = document.getElementById('container');

var recordVideo;
record.onclick = function () {
    record.disabled = true;

    navigator.getUserMedia = navigator.getUserMedia || navigator.mozGetUserMedia || navigator.webkitGetUserMedia;
    navigator.getUserMedia({
        audio: true,
        video: true
    }, function (stream) {
        preview.srcObject = stream;
        //  preview.src = window.URL.createObjectURL(stream);
        preview.play();

        recordVideo = RecordRTC(stream, {
            type: 'video'
        });

        recordVideo.startRecording();

        stop.disabled = false;
    }, function (error) {
        alert(error.toString());
    });
};

var fileName;
stop.onclick = function () {
    record.disabled = false;
    stop.disabled = true;

    preview.src = '';

    fileName = (Math.round(Math.random() * 99999999) + 99999999) + '.webm';

    recordVideo.stopRecording(function () {
        PostBlob(recordVideo.getBlob());
    });

    deleteFiles.disabled = false;
};

deleteFiles.onclick = function () {
    deleteAudioVideoFiles();
};

function deleteAudioVideoFiles() {
    deleteFiles.disabled = true;
    if (!fileName) return;
    var formData = new FormData();
    formData.append('delete-file', fileName);
    xhr('/RecordRTC/DeleteFile', formData, null, null, function (response) {
        console.log(response);
    });
    fileName = null;
    container.innerHTML = '';
}

function xhr(url, data, progress, percentage, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            callback(request.responseText);
        }
    };

    if (url.indexOf('/RecordRTC/DeleteFile') == -1) {
        request.upload.onloadstart = function () {
            percentage.innerHTML = 'Upload started...';
        };

        request.upload.onprogress = function (event) {
            progress.max = event.total;
            progress.value = event.loaded;
            percentage.innerHTML = 'Upload Progress ' + Math.round(event.loaded / event.total * 100) + "%";
        };

        request.upload.onload = function () {
            percentage.innerHTML = 'Saved!';
        };
    }

    request.open('POST', url, true);
    request.send(data);
}

window.onbeforeunload = function () {
    if (!!fileName) {
        deleteAudioVideoFiles();
        return 'It seems that you\'ve not deleted audio/video files from the server.';
    }
};
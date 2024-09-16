function imageFiles() {
    return 'imgProducts';
}

Dropzone.options.shopDropZone = {
    url: $('#shopDropZone').attr('dropzone-url'),
    //parallelUploads: 1,
    uploadMultiple: true,
    maxFilesize: 10000000,//byte
    paramName: imageFiles,
    maxFiles: 5,
    acceptedFiles: '.png, .jpeg',
    init: function () {
        this.on("successmultiple", function (file, response) {
            console.log(response);
            if (response.status == "Success") {
                ShowMessage('پیغام موفقیت', 'تصویر مورد نظر با موفقیت ثبت شد', 'success');
                window.location.replace('https://tasland.ir/admin/Product/ShowProduct');

            }
        })
    }
}
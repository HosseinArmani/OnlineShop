﻿function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-left',
        showDuration: 6000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-Form").submit();
}
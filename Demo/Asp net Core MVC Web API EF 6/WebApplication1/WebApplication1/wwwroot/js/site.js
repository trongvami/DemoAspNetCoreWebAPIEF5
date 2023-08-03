// ---------- Role Function ---------- //
// ---------- Cate Function ---------- //

ShowInPupupRole = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#Form-Modal-AddorEditRole .modal-content').html(res);
            $('#Form-Modal-AddorEditRole .modal-title').html(title);
            $('#Form-Modal-AddorEditRole').modal('show');
            //$('.modal-dialog').draggable({
            //    handle: ".modal-header"
            //});
        }
    })
}

jQueryAjaxPostModalRole = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-role').html(res.html);
                    $('#Form-Modal-AddorEditRole .modal-content').html('');
                    $('#Form-Modal-AddorEditRole .modal-title').html('');
                    $('#Form-Modal-AddorEditRole').modal('hide');
                }
                else
                    $('#Form-Modal-AddorEditRole .modal-content').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
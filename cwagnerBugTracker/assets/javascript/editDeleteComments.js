////SweetAlert for delete confirmation
function commentDelete(id, url) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this comment!",
        type: "error",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Delete",
        closeOnConfirm: false,
    },
    function (data) {
        return $.post(url,
            {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            function (response) {
                swal({
                    type: 'success',
                    title: 'Comment successfully deleted!',
                })
                $("div[data-id='" + id + "']").remove();
            });
    });
};

function toggleCommentEdit(id) {
    $('#editForm-' + id).toggle();
    $('#comment-body-' + id).toggle();
}

//DELETE ATTACHMENT
function attachmentDelete(id, url) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this file!",
        type: "error",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Delete",
        closeOnConfirm: false,
    },
    function (data) {
        return $.post(url,
            {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            function (response) {
                swal({
                    type: 'success',
                    title: 'File successfully deleted!',
                })
                $("div[data-attachment-id='" + id + "']").remove();
            });
    });
};
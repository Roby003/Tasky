$(function () {
    var placeholderElement = $('#modal-placeholder');
    //load the modal in place of the placeholder div when button is clicked
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        //post form data
  /*      console.log(event)
        console.log(event.target.id)*/
        // Get the ID of the clicked button
        var buttonId = event.target.id;
        //use the button id to select the correct form
        var formClass = '.modal-body form#' + buttonId;
        var form = $(formClass);

        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        console.log(dataToSend)
        $.post(actionUrl, dataToSend).done(function (data) {
            var newForm = $(data).find(formClass);

            // Replace the content of the existing form
            form.replaceWith(newForm);
            // find IsValid input field and check it's value
            // if it's valid then hide modal window
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
                location.reload();
            }
        });

    });
  
});

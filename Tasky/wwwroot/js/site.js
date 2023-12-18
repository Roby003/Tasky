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
    $(function () {


        placeholderElement.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();
            //post form data

            // Get the ID of the clicked button
            var buttonId = event.target.id;
            //use the button id to select the correct form
            if (buttonId == 'form_add' || buttonId=='test_id')
                var formClass = '.modal-body form#' + buttonId;
            else
                var formClass = '.modal-footer form#' + buttonId;

            var form = $(formClass);

            var actionUrl = form.attr('action');
            var dataToSend = form.serialize();
            console.log(dataToSend);
            $.post(actionUrl, dataToSend).done(function (data) {
                var newForm = $(data).find(formClass);
                // Replace the content of the existing form
                form.replaceWith(newForm);
                // find IsValid input field and check it's value
                // if it's valid then hide modal window
                var isValid = newForm.find('[name="IsValid"]').val() == 'True';

                if (isValid) {
                    if (buttonId == 'test_id') {//taskModal
                        placeholderElement.find('.modal').modal('hide');
                        location.reload();
                    }
                    else {//taskShowModal

                        var messageContainer = placeholderElement.find('#commentMessage');
                        messageContainer.text('Comment saved successfully');
                        //send get request to show the newly created comment
                        var newFooter = $(data).find('.modal-footer');
                        var footer = $('.modal-footer')
                        footer.replaceWith(newFooter)
                        setTimeout(function () {
                            messageContainer.text(''); // Clear the message after a few seconds
                        }, 3000); // Adjust the duration as needed
                    }
                }

            });

        });
    })
})

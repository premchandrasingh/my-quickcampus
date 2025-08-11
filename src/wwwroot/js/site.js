$(document).ready(function () {
    $('.isnew-checkbox').change(function () {
        var $tr = $(this).parents('tr');
        if ($(this).is(':checked')) {
            $tr.addClass('new-work')
        } else {
            $tr.removeClass('new-work')
        }
    });


    $('#captureScreen').click(function () {
        // captureTargetId is declared in the page
        html2canvas(document.querySelector(captureTargetId)).then(canvas => {
            // The 'canvas' object now contains the rendered image of your div.
            // You can append it to the body, convert it to an image, or save it.

            // Example: Appending the canvas to the body
            //document.body.appendChild(canvas);

            // Example: Converting to a data URL (e.g., for saving or displaying in an <img>)
            let imageDataURL = canvas.toDataURL("image/png");
            //console.log(imageDataURL); // Log the data URL

            const link = document.createElement('a')
            link.href = imageDataURL
            link.download = captureFileName
            document.body.appendChild(link)
            link.click()
            document.body.removeChild(link)
        });
    });

});
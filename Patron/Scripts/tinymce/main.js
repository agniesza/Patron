tinymce.init({
    selector: 'textarea',
    /* display statusbar */
    statubar: true,

    /* plugin */
    plugins: [
        "autolink link image charmap print preview hr anchor pagebreak",
        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
        "save table contextmenu directionality emoticons template textcolor"
    ],

    /* toolbar */
    toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",

    menubar: false,
    toolbar_items_size: 'large',
    min_height: 500,
    max_height: 800,
    convert_newlines_to_brs: false,
   // statusbar: false,
    relative_urls: false,
    remove_script_host: false,
    language: 'pl',

});
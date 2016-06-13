//
//	jQuery Validate example script
//
//	Prepared by David Cochran
//
//	Free for your use -- No warranties, no guarantees!
//

jQuery(document).ready(function () {

    // Validate
    // http://bassistance.de/jquery-plugins/jquery-plugin-validation/
    // http://docs.jquery.com/Plugins/Validation/
    // http://docs.jquery.com/Plugins/Validation/validate#toptions

    $("#accordian h3").click(function () {
        //slide up all the link lists
        $("#accordian ul ul").slideUp();
        //slide down the link list below the h3 clicked - only if its closed
        if (!$(this).next().is(":visible")) {
            $(this).next().slideDown();
        }

    });










    // for left show-div hide 
    /*ddiconmenu.docinit({ // initialize an Icon Menu
    menuid:'myiconmenu', //main menu ID
    easing:"easeInOutCirc",
    dur:500 //<--no comma after last setting
    });*/


    // for validation
    /*jQuery('#contact-form').validate({
    rules: {
    name: {
    minlength: 2,
    required: true
    },
    email: {
    required: true,
    email: true
    },
    subject: {
    minlength: 2,
    required: true
    },
    message: {
    minlength: 2,
    required: true
    }
    },
    highlight: function(element) {
    jQuery(element).closest('.control-group').removeClass('success').addClass('error');
    },
    success: function(element) {
    jQuery(element).parent().children('label').remove();
				
    }
    });
      
      
      
    // for tooltip
    jQuery("a.tooltipleftmenu").tooltip({
    placement : 'bottom'
    });
    */
    //tool tip
    jQuery("a.tooltipleftmenu").tooltip();
    

    // for left menu show hide
    // var menuLeft = document.getElementById('cbp-spmenu-s1')
    // body = document.body;
    // showLeft.onclick = function () {
    //classie.toggle(this, 'active');
    // classie.toggle(menuLeft, 'cbp-spmenu-open');
    // disableOther('showLeft');
    //};		

    //        $(document).ready(function () {
    //            $("#accordian h3").click(function () {
    //                //slide up all the link lists
    //                $("#accordian ul ul").slideUp();
    //                //slide down the link list below the h3 clicked - only if its closed
    //                if (!$(this).next().is(":visible")) {
    //                    $(this).next().slideDown();
    //                }
    //            })
    //        })			

});     // end document.ready
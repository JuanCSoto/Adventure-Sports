$(window).load(function () {

    setTimeout(function () {

        $('.seccion').css('height', $(window).height());

        $("#side-menu").css('top', (($(window).height() - $("#side-menu").height()) * .5) + "px")

        // MENU PPAL
        if ($(".flexnav").length) {
            $(".flexnav").flexNav();
        }

        // FIXED MENU
        $(function () {
            $('#header-menu').data('size', 'big');
            checkScrollAndResize();
        });

        $(window).scroll(function () {
            checkScrollAndResize();
        });

        function checkScrollAndResize() {
            var scrollTop = $(document).scrollTop();
            var size = $('#header-menu').data('size');
            if (size == 'big' && scrollTop > 50)
                doSmall();
            else if (scrollTop == 0)
                doBig();
        }

        function doSmall() {
            $('#header-menu').css('top', -112)
        }

        function doBig() {
            $('#header-menu').css('top', 0)
        }

        // SLIDER
        if ($('#slider').length) {
            $('#slider').bjqs({
                height: '100%',
                width: '100%',
                responsive: true,
                animspeed: 7000,
            });
        }

        // STELLAR
        if ($(window).width() >= 1025) {
            $(window).stellar({
                responsive: true,
            });
        }

        // CENTRADO VERTICAL
        function centradoVertical() {
            $('.seccion .centrado-vertical').each(function (index, element) {
                var alturaContainer = $(element).height() / 2;
                $(element).css('margin-top', -alturaContainer);
            });
        }

        // SECTION WIPE
        function seccionWipe() {
            var controller = new ScrollMagic.Controller({
                globalSceneOptions: {
                    triggerHook: 'onLeave'
                }
            });
            var slides = document.querySelectorAll(".seccion.superpuesto");
            for (var i = 0; i < slides.length; i++) {
                new ScrollMagic.Scene({
                    triggerElement: slides[i]
                })
                .setPin(slides[i])
                .addTo(controller);
            }
        }

        if ($(window).width() >= 992) {
            centradoVertical();
        }

        // RESIZE WINDOWS
        $(window).resize(function () {
            if ($(window).width() >= 992) {
                centradoVertical();
            }
            if ($(window).width() <= 992) {
                $('.seccion .centrado-vertical').css('margin-top', 0);
            }
            //set side-menu
            $("#side-menu").css('top', (($(window).height() - $("#side-menu").height()) * .5) + "px")
            $('video').css('height', $(window).height() + "px")
        });

        // ANIMATE
        $(window).scroll(function () {
            $('.seccion').each(function () {
                var imagePos = $(this).offset().top;
                var topOfWindow = $(window).scrollTop();
                if (imagePos < topOfWindow + 400) {
                    $(this).addClass('activate');
                    $(this).addClass($(this).data('fx'));
                }
            });
        });

        // MODAL POSTULA IDEA

        $('a#postular').click(function () {
            $('#modal-postula-idea').fadeIn('normal');
            $('#overlay').fadeIn('normal');
            $('body').addClass('no-scroll');
        });

        $('#modal-postula-idea .btn-cerrar').click(function () {
            $('#modal-postula-idea').fadeOut('normal');
            $('#overlay').fadeOut('normal');
            $('body').removeClass('no-scroll');
        });

        initOptionMenu();
        $('video').css('height', $(window).height() + "px")

    }, 500);
    //scroll bullets parallax active
    ScrollBulletsParallax();

});
var ScrollBulletsParallax = function () {
    $(window).scroll(function () {
        $.each($('.seccion'), function (index, value) {
            if ($(window).scrollTop() > 150) {
                $("#side-menu-inner #seccion1 li").addClass("active");
                $("#side-menu-inner #seccion3 li").removeClass("active");
                $("#side-menu-inner #seccion4 li").removeClass("active");
                $("#side-menu-inner #seccion5 li").removeClass("active");
                $("#side-menu-inner #seccion7 li").removeClass("active");
                $("#side-menu-inner #seccion8 li").removeClass("active");
            }
            if ($(window).scrollTop() > 749) {
                $("#side-menu-inner #seccion1 li").removeClass("active");
                $("#side-menu-inner #seccion3 li").addClass("active");
                $("#side-menu-inner #seccion4 li").removeClass("active");
                $("#side-menu-inner #seccion5 li").removeClass("active");
                $("#side-menu-inner #seccion7 li").removeClass("active");
                $("#side-menu-inner #seccion8 li").removeClass("active");
            }
            if ($(window).scrollTop() > 1348) {

                $("#side-menu-inner #seccion1 li").removeClass("active");
                $("#side-menu-inner #seccion3 li").removeClass("active");
                $("#side-menu-inner #seccion4 li").addClass("active");
                $("#side-menu-inner #seccion5 li").removeClass("active");
                $("#side-menu-inner #seccion7 li").removeClass("active");
                $("#side-menu-inner #seccion8 li").removeClass("active");

            }
            if ($(window).scrollTop() > 1947) {
                $("#side-menu-inner #seccion1 li").removeClass("active");
                $("#side-menu-inner #seccion3 li").removeClass("active");
                $("#side-menu-inner #seccion4 li").removeClass("active");
                $("#side-menu-inner #seccion5 li").addClass("active");
                $("#side-menu-inner #seccion7 li").removeClass("active");
                $("#side-menu-inner #seccion8 li").removeClass("active");
            }
            if ($(window).scrollTop() > 2546) {
                $("#side-menu-inner #seccion1 li").removeClass("active");
                $("#side-menu-inner #seccion3 li").removeClass("active");
                $("#side-menu-inner #seccion4 li").removeClass("active");
                $("#side-menu-inner #seccion5 li").removeClass("active");
                $("#side-menu-inner #seccion7 li").addClass("active");
                $("#side-menu-inner #seccion8 li").removeClass("active");
            }
            if ($(window).scrollTop() > 3145) {
                $("#side-menu-inner #seccion1 li").removeClass("active");
                $("#side-menu-inner #seccion3 li").removeClass("active");
                $("#side-menu-inner #seccion4 li").removeClass("active");
                $("#side-menu-inner #seccion5 li").removeClass("active");
                $("#side-menu-inner #seccion7 li").removeClass("active");
                $("#side-menu-inner #seccion8 li").addClass("active");
            }
        });
    });
};
function updateSideMenu() {
    //$('.seccion')
}

/*
 *
 *
 * initOptionMenu()
 * Agrega evento de clic a las opciones del menu
 *
*/
function initOptionMenu() {
    $('#side-menu-inner a').on("click", function (event) {

        var current = this;
        event.preventDefault();

        $('#side-menu-inner a li').removeClass('active');
        $(current).find('li').addClass('active');

        //scrollWindowTo($(this)[0].hash/*,$(this).data('hash')*/);    
        scrollWindowTo($(this)[0].hash, $(this).data('hash'));
    })
}

/*
 *
 *
 * scrollWindowTo()
 * efecto de scroll de la ventana
 *
*/
function scrollWindowTo(element, hash) {
    if ($(element).length) {
        var top = $(element).offset().top;
        TweenMax.to(window, 1, {
            scrollTo: { y: top }, ease: Expo.easeOut, onComplete: function () {
                if (hash)
                    window.location.hash = hash;
            }
        });
    }
}


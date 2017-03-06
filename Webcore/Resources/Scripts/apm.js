var scroll = 0;
var contenido_textarea = '';
var facebookCicle = 0;
var googleWindow;
var googleLoaded = false;
var globalCallback = null;
var twitterWindow;
var paginCicle = 0;
var paginLayerCicle = 0;
var map;
var ideaMap;
var marker;
var infoWindow;
var bannerPosition = 1;
var bannerPositionPan = 1;
var bannerAnimation = false;
var bannerAnimationPan = false;
var bannerTimeout = 0;
var playerReady = false;
var ideaColumnWidth = 320;
var masonryInstance = null;
var stopPulses = true;
var iFrameImageLoaded = false;
var isMobile = (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase()));
var isMac = (/iphone|ipad|ipod/i.test(navigator.userAgent.toLowerCase()));
var lastAjaxOptions = null;
var $boxContent = [];
var resizeTime = null;
var viewport = false;

String.prototype.replaceBetween = function (start, end, what) {
    return this.substring(0, start) + (start > 0 ? ' ' : '') + what + this.substring(end);
};

$.expr[':'].containsIgnoreCase = function (n, i, m) { return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0; };
$.expr[':'].noContainsIgnoreCase = function (n, i, m) { return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) == -1; };

(function ($) {
    $.fn.hashtags = function () {
        var $target = $(this);
        var textLength = $target.val().length * 2;
        $target.wrap('<div class="jqueryHashtags"><div class="highlighter"></div></div>').unwrap().before('<div class="highlighter"></div>').wrap('<div class="typehead"></div></div>');
        $target.addClass("theSelector");
        $target.focus();
        $target[0].setSelectionRange(textLength, textLength);
        $target.trigger('propertychange');
        $target.autosize({ append: " " });

        var markHashTag = function ($textarea) {
            var str = $textarea.val();
            $textarea.parent().parent().find(".highlighter").css("width", $textarea.css("width"));
            str = str.replace(/\n/g, '<br>');
            if (!str.match(/(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?#([a-zA-Z0-9]+)/g)) {
                if (!str.match(/#([a-zA-Z0-9]+)#/g)) {
                    str = str.replace(/#([a-zA-Z0-9]+)/g, '<span class="hashtag">#$1</span>');
                }
                else {
                    str = str.replace(/#([a-zA-Z0-9]+)#([a-zA-Z0-9]+)/g, '<span class="hashtag">#$1</span>');
                }
            }
            $textarea.parent().parent().find(".highlighter").html(str);
        }

        $target.on("keyup", function () {
            var $textarea = $(this);
            markHashTag($textarea);
        });

        $target.parent().prev().on('click', function () {
            $(this).parent().find(".theSelector").focus();
        });

        var hashTagClick = function () {
            var $item = $(this);
            cursorCurrentWord($target, $item.html());
            markHashTag($target);
            hideOptions();
        };

        var showHashTag = function ($target, $container, value) {
            if ($target.val().length > 0) {
                $container.find('.hashtag-item').hide();
                var $matchs = $container.find('.hashtag-item:containsIgnoreCase("' + value + '")').slice(0, 5).show();

                if ($matchs.length > 0) {
                    $container.show();
                }
                else {
                    $container.hide();
                }
            }
            else {
                $container.hide();
            }
        };

        var lastSearch = '';
        var searchHashTag = function ($target, value) {
            var $container = $target.parent().parent().parent().find('.hashtag-container');

            if (value.length >= 2) {
                if (lastSearch != value.substring(0, 2)) {
                    lastSearch = value.substring(0, 2);

                    $container.empty();

                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: path + '/idea/gethashtags',
                        data: {
                            value: lastSearch
                        }
                    }).done(function (json) {
                        if (json.result) {
                            $.each(json.hashTags, function () {
                                $container.append($('<div class="hashtag-item" />').html(this.Value).mousedown(hashTagClick));
                            });

                            showHashTag($target, $container, value);
                        }
                        else {
                            systemAlert('¡Uups!', 'Esta acción no se pudo realizar.');
                        }
                    });
                }
                else {
                    showHashTag($target, $container, value);
                }
            }
        };

        var hideOptions = function () {
            var $container = $target.parent().parent().parent().find('.hashtag-container');
            $container.hide();
        }

        $target.on('click', function () {
            var $textarea = $(this);
            var current = cursorCurrentWord($textarea);
            if (current.length && current[0] == '#') {
                searchHashTag($textarea, current);
            }
            else {
                hideOptions();
            }
        });

        $target.on('keyup', function () {
            var $textarea = $(this);
            var current = cursorCurrentWord($textarea);
            if (current.length && current[0] == '#') {
                searchHashTag($textarea, current);
            }
            else {
                hideOptions();
            }
        });

        $target.blur(function () {
            hideOptions();
        });

        $target.trigger('keyup');
    };
})(jQuery);

if ($('#is-ie').length) {
    $['browser'] = { msie: true };
}
else {
    $['browser'] = { msie: false };
}

// EVENTS, INIT & VALIDATORS

// trigger after all ajax call
$(document).ajaxSuccess(ajaxSuccessController);
$(document).ajaxStart(ajaxStartController);

$(document).ajaxSend(function (event, xhr, options) {
    if (options.url + '@' + options.data != lastAjaxOptions) {
        lastAjaxOptions = options.url + '@' + options.data;
    }
    else {
        xhr.abort();
    }
}).ajaxComplete(function (event, xhr, options) {
    if (options.url + '@' + options.data == lastAjaxOptions) {
        lastAjaxOptions = null;
    }
});

function ajaxStartController() {

}

function ajaxController(active) {
    if (active) {
        $(document).unbind('ajaxSuccess').ajaxSuccess(ajaxSuccessController);
        $(document).unbind('ajaxStart').ajaxStart(ajaxStartController);
    }
    else {
        $(document).unbind('ajaxSuccess');
        $(document).unbind('ajaxStart');
    }
}

function ajaxSuccessController() {
    $(document).unbind('ajaxSuccess');
    $(document).unbind('ajaxStart');

    if ($('[placeholder]').length) {
        renderPlaceholder();
    }

    updateUserBlock(function () {
        $('.menu-ingresar').unbind('click');
        $('.menu-ingresar').click(function () { showEntry(); });
        if ($('.show-menu-user-block').length) {
            $('.show-menu-user-block').unbind('click').click(showMenuUserBlock);
        }
        if ($('.show-notifications').length) {
            $('.show-notifications').unbind('click').click(showLatestNotifications);
        }
        if ($(".botoningresa").length) {
            $(".botoningresa").unbind('click').click(showEntry)
        }
        imageMargin('.usuario-block img', 35, 35);
        setTimeout(function () {
            $(document).unbind('ajaxSuccess').ajaxSuccess(ajaxSuccessController);
            $(document).unbind('ajaxStart').ajaxStart(ajaxStartController);
        }, 10);
    });
}

function setViewport() {
    if (isMac) {
        //$('meta[name="viewport"]').attr('content', 'width=device-width, initial-scale=1, user-scalable=no');
    }
    else {
        if (window.innerWidth < $(document).outerWidth()) {
            $('meta[name="viewport"]').attr('content', 'initial-scale=1').attr('content', 'width=device-width');
        }

        setTimeout(setViewport, 10);
    }
}

jQuery.validator.unobtrusive.adapters.add("checkboxtrue", function (options) {
    if (options.element.tagName.toUpperCase() == "INPUT" && options.element.type.toUpperCase() == "CHECKBOX") {
        options.rules["required"] = true;
        if (options.message) {
            options.messages["required"] = options.message;
        }
    }
});

$(document).ready(function () {
    window.addEventListener("orientationchange", function (event) {
        resizeColorBox();
        $('html').hide(0).show(0);
        if ($('[data-crop-height]').length) {
            cropText();
        }
        setViewport();
    }, false);

    //$(window).load(function () {
    setViewport();
    //});

    if ($('#compartir-home').length) {
        $('#compartir-home').unbind('click').click(showCompartir);
    }

    if ($('#send-manual-email').length) {
        $('#send-manual-email').click(sendManualEmail);
    }

    if ($('#show-phone-login').length) {
        $('#show-phone-login').click(function () {
            $('#phone-login-fields').toggle();
            $('#email-login-fields').toggle();
            $('#show-phone-login').toggleClass('phone-icon').toggleClass('phone-icon-gray');
        });

        $('.mm2-ico-cel2').click(function () {
            $('#phone-login-fields').toggle();
            $('#email-login-fields').toggle();
            $('#show-phone-login').toggleClass('phone-icon').toggleClass('phone-icon-gray');
        });
    }

    if ($('#date-select').length) {
        if ($('#date-select option[selected]').length == 0) {
            var $option = $('<option />');
            $option.html($('#statistics-date').val()).attr('selected', 'selected');
            $('#date-select').append($option);
        }

        $('#date-select').change(function () {
            var $this = $(this);
            if ($this.val() == '0') {
                $('#statistics-date').click();
            }
            else if ($this.val().length) {
                redirect(path + '/estadisticas' + statisticsView + '/' + $this.val() + '-' + $this.attr('data-now'));
            }
            else {
                redirect(path + '/estadisticas' + statisticsView);
            }
        });
    }

    if ($('.show-menu-user-block').length) {
        $('.show-menu-user-block').unbind('click').click(showMenuUserBlock);
    }

    if ($('#main-notification-list').length) {
        notificationsPaging(0);
    }

    if ($('.show-notifications').length) {
        $('.show-notifications').unbind('click').click(showLatestNotifications);
    }

    if ($('.select-style-box').length) {
        $.each($('.select-style-box'), function (index, obj) {
            var $target = $(obj);
            $target.find('.select-text').html($target.find('.select-style-profile option:selected').text());

            $target.find('.select-style-profile').change(function () {
                var $select = $(this);
                $target.find('.select-text').html($select.find('option:selected').text());
            });
        });

        if ($('#CountryId').length) {
            $('#CountryId').change(function () {
                $('#CityId').empty().append($("<option />").val('').text('Cargando...')).change();
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: path + '/registro/getcities',
                    data: {
                        countryId: $('#CountryId').val(),
                    }
                }).done(function (json) {
                    if (json.result) {
                        var $cities = $('#CityId');
                        $cities.empty();
                        $.each(json.cities, function () {
                            if (json.lenguaje == 1) {
                                $cities.append($("<option />").val(this.CityID).text(this.NameEn));
                            }
                            else {
                                $cities.append($("<option />").val(this.CityID).text(this.NameEs));
                            }                            
                        });
                        $cities.change();
                    }
                    else {

                    }
                });
            });
        }
    }

    if ($('[data-only-number]').length) {
        $('[data-only-number]').keypress(onlyNumbers);
    }

    if ($('[data-no-number]').length) {
        $('[data-no-number]').keypress(noNumbers);
    }

    if ($('#search-result').length) {
        if ($('#send-search').length) {
            $('#send-search').click(search).click();
        }



        if ($('#community-filters').length) {
            $('#community-filters div').click(filterCommunity);
            $('#community-filters div.selected').click();
        }
    }

    if ($('.comp-pulso').length) {
        $('.comp-pulso').unbind('click').click(showCompartir);
    }

    if ($('.mm2-create-news').length) {
        if (isMobile) {
            $('.mm2-create-news').remove
        }
        else {
            $('.mm2-create-news').unbind('click').click(function () {
                newBlogEntry();
            });
        }
    }

    if ($('.edit-blog').length) {
        $('.edit-blog').unbind('click').click(function () {
            var $this = $(this);
            editBlogEntry($this.attr('data-id'));
        });
    }

    if ($('.delete-blog').length) {
        $('.delete-blog').unbind('click').click(function () {
            var $this = $(this);
            systemConfirm($this.data('title-modal'), $this.data('message-modal'), function (result, close) {
                if (result) {
                    deleteBlogEntry($this.attr('data-id'));
                }

                if (close) {
                    closeColorbox();
                }
            });
        });
    }

    if ($('.delete-blog-comment').length) {
        $('.delete-blog-comment').unbind('click').click(function () {
            var $this = $(this);
            systemConfirm($this.data('title-modal'), $this.data('message-modal'), function (result, close) {
                if (result) {
                    deleteBlogCommentEntry($this.attr('data-id'));
                }

                if (close) {
                    closeColorbox();
                }
            });
        });
    }

    if ($('#show-email-setting').length) {
        $('#show-email-setting').click(function () {
            $('#email-setting-container').toggle(400)
        });
    }

    if ($('#send-email-setting').length) {
        $('#send-email-setting').click(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: path + '/perfil/saveemailsetting',
                data: $("#email-setting").serialize()
            }).done(function (json) {
                if (json.result) {
                    $('#email-setting-container').hide(500)
                }
            });
        });
    }

    if ($('#search-text').length) {
        $('#search-text').keyup(function (event) {
            if (event.keyCode == 13) {
                $('#send-search').click();
            }
        });
    }

    if ($('#Age').length) {
        $('#Age').keypress(onlyNumbers);
    }

    if ($('[data-crop-height]').length) {
        cropText();
    }

    if ($('[placeholder]').length) {
        renderPlaceholder();
    }

    $(window).resize(function () {
        clearTimeout(resizeTime);
        resizeTime = setTimeout(function () {

        }, 100);
    });

    $('.pregunta-block-p').click(function () {
        $click = $(this);

        var $parent = $click.parent()
        var $title = $parent.find('.pregunta-block-p');
        var $target = $parent.find('.pregunta-block-text');
        var $arrow = $title.find('img')

        if ($parent.attr('data-visible') == 'true') {
            var height = 50;
            $parent.attr('data-visible', 'false');

            $parent.animate({ 'height': height }, 400, function () {
                $parent.css('background-color', '#f2f2f2');
                $title.css('border-color', '#fff');
                $arrow.attr('src', $arrow.attr('src').replace('p1', 'p2'));
                $(document).scrollTop($click.position().top + height);
            });
        }
        else {
            var height = $target.height() + 50;
            $parent.attr('data-visible', 'true');

            $parent.animate({ 'height': height }, 400, function () {
                $parent.css('background-color', '#fff');
                $title.css('border-color', '#f2f2f2');
                $arrow.attr('src', $arrow.attr('src').replace('p2', 'p1'));
                $(document).scrollTop($click.position().top + height);
            });
        }
    });

    //blog en pagina (no layer)
    $('#layer-comment-text').keydown(commentarioEnterKey);
    $('#blog-comment-text').keydown(commentarioBlogEnterKey);

    //auto size for textareas
    //  $('textarea').autosize();
    var textAreas = document.querySelector('textarea');

    //vote question
    $('.opcion-votar').click(function () { answerVote(this); });
    $('.opcion-votar2').click(function () { answerVote(this); });

    //animate bars
    animatebar();

    $('#clean-user').click(function () {
        cleanUser($(this));
    });

    // registry load logic 
    imageMargin('#user-image-file', 58, 58);
    $('#ubicacionboton').html($(".ubicacion-block-h2[data-id='" + $('#LocationId').val() + "'][data-type='" + $('#LocationType').val() + "']").html());
    $('#change-password-button').click(function (event) {
        $('#change-password-wraper').toggle();
        resizeColorBox($(document).width(), $(document).height());
    });
    var twitterToken = readCookie('twitter-token');
    if (twitterToken) {
        $('#twitterToken').val(twitterToken);
        eraseCookie('twitter-token');
        $('#register-password-wrapper').hide();
    }

    var googleToken = readCookie('google-token');
    if (googleToken) {
        $('#googleToken').val(googleToken);
        eraseCookie('google-token');
        $('#register-password-wrapper').hide();
    }

    var linkedinToken = readCookie('linkedin-token');
    if (linkedinToken) {
        $('#linkedinToken').val(linkedinToken);
        eraseCookie('linkedin-token');
        $('#register-password-wrapper').hide();
    }

    var facebookToken = readCookie('facebook-token');
    if (facebookToken) {
        $('#facebookToken').val(facebookToken);
        eraseCookie('facebook-token');
        $('#register-password-wrapper').hide();
    }

    var phone = readCookie('phone-registry');
    if (phone) {
        $('#Phone').val(phone);
        eraseCookie('phone-registry');
        $('#register-phone-wrapper').show();
        $('#register-password-wrapper').hide();
        $('#register-email-wrapper').hide();
        $('#form-captcha').show();
    }

    var userImage = readCookie('user-image')
    if (userImage) {
        $('#user-image-file').attr('src', userImage);
        $('#imageName').val(readCookie('user-image'));
        imageMargin('#user-image-file', 58, 58);
        eraseCookie('user-image');
    }

    if (readCookie('user-name')) {
        $('#Names').val(readCookie('user-name'));
        eraseCookie('user-name');
    }
    if (readCookie('user-mail')) {
        $('#Email').val(readCookie('user-mail'));
        eraseCookie('user-mail');
    }

    if ($('#registry-successful').length > 0) {
        if (window.self === window.top) {
            if (globalCallback) {
                globalCallback();
            }
            else {
                redirect(path)
            }
        }
        else {
            if (parent.globalCallback) {
                parent.globalCallback();
            }
            else {
                parent.redirect(parent.document.location.href);
            }
        }

    }

    if ($('#update-information-successful').length > 0) {
        if (window.self === window.top) {
            redirect(path + '/perfil')
        }
        else {
            parent.redirect(parent.document.location.href);
        }
    }

    if ($('#contact-us-successful').length > 0) {
        var MessageSucces = $('#contact-us-successful').attr('data-messageexito');
        parent.systemAlert('CONTÁCTANOS', MessageSucces);
    }
    // end - registry load logic 

    // small entry - new functions are required (current ones are opening a layer)
    $('.desplegables .googlei').click(function () { googleLogin() });
    $('.desplegables .twitteri').click(function () { twitterLogin() });
    $('.desplegables .facebooki').click(function () { facebookLogin() });
    $('.desplegables .linkedini').click(function () { linkedInLogin() });
    $('.desplegables .alerta-aceptar').click(function (event) {
        entryUser('#smallRegistro');
    });
    $('.desplegables .registrarseb').click(showRegistry);
    $('.desplegables #recovery').click(showRecovery);
    // end small entry
    if (parent.frames.length > 0) {
        resizeColorBox($(document).width(), $(document).height())
    }
    if ($('#reset-password').length > 0) {
        showResetPassword($('#reset-password').attr('data-token'));
    }
    $('.unirseboton').click(function () {
        joinChallenge(this);
    });
    if ($('.idea-versus').length > 0) {
        $('.idea-versus').click(function (event) {
            var id = $(this).attr('data-id');
            showVersus(id, true);
        });

        //checkVersus();
    }
    $('#user-update-links a').click(showSocialLink);
    if ($('.update-profile').length) {
        $('.update-profile').click(function () {
            var $target = $(this);
            showUpdate($target.attr('data-option'));
        });
    }
    $("#home-images .banner-img ").mouseenter(enterBanner);
    $("#home-images .banner-img ").mouseout(outBanner);
    $("#home-images .banner-img").click(showPan);
    $('.seccion-filtro').click(showSectionFilter);
    $('#activoSubmenu div').click(sectionFilter);
    $(".img5").click();
    $('body').bind('onorientationchange', function (event) {
        // si se quiere agregar algo para cuando el dispositivo rote aca se pone.
    });
    imageMargin('.usuario-block img', 35, 35);
    imageMargin('.perfil-u-img img', 45, 45);
    imageMargin('.top-item-img', 40, 40);
    // upload image registry
    $("#user-file-wraper").mousemove(function (event) {
        var offTop = $(this).offset().top;
        width = $(this).find("input").width();
        $(this).find("input").css({
            left: event.pageX - width + 30,
            top: event.pageY - offTop - 10
        })
    });
    $('#userFile').change(function () {
        parent.iFrameImageLoaded = false;
        parent.$('#error-file').hide();
        $('#user-form-file').submit();
    });
    $('#editableFile').change(function () {
        parent.iFrameImageLoaded = false;
        $('#editable-form-file').submit();
    });
    // end upload image registry

    // upload image idea
    $("#idea-file-wraper").mousemove(function (event) {
        var offTop = $(this).offset().top;
        width = $(this).find("input").width();
        $(this).find("input").css({
            left: event.pageX - width + 30,
            top: event.pageY - offTop - 10
        })
    });
    $('#ideaFile').change(function () {
        parent.iFrameImageLoaded = false;
        $('#idea-form-file').submit();
    });
    // end upload image idea
    $('#reto-comment-text').keydown(commentarioEnterKey);
    $('#idea-comment-text').keydown(commentarioEnterKey);
    $('.comentar-coment').unbind('click');
    $('.comentar-coment').click(commentarioEnterKey);
    $('.comentar-blog').unbind('click');
    $('.comentar-blog').click(commentarioBlogEnterKey);
    $('#registro-aceptar').click(function (event) { validateUserForm() });
    $('.alerta-cerrar').click(function () {
        if (InFrame()) {
            closeColorbox();
        }
        else {
            window.history.back();
        }
    });
    $('.menu-ingresar').click(function () { showEntry(); });
    $('.botoningresa').click(function () { showEntry(); });
    $('.terms').click(showTerms);
    $('#contact-us').click(showContactUs);
    $('#registry-terms').click(showTermsRegistry);
    if ($('#terms').length > 0) {
        if ($(window).width() > 400) {
            $('#terms').find('#scrollbar1').css('width', '740');
            $('#terms').find('#scrollbar1 .viewport').css('width', '715');
            $('#terms').find('#scrollbar1').tinyscrollbar();
        }
        else {
            $('#terms').removeClass('alerta').width('320px');
            $('#terms').find('#scrollbar1').css('width', '320');
            $('#terms').find('#scrollbar1 .viewport').css('height', 'auto').css('width', '310');
            $('#terms').find('#scrollbar1').tinyscrollbar();
            $('#terms').find('#scrollbar1 .overview').css('position', 'relative');
            $('#terms').find('#scrollbar1 .scrollbar').hide();
            $('table').attr('width', '280px');
            $('tr').each(function () { $(this).children('td:first').attr('width', '120') })
            $('tr').each(function () { $(this).children('td:nth-child(2)').attr('width', '100') })
        }
    }
    $('#registry-privacy').click(showPrivacyRegistry);
    $('.privacy').click(showPrivacy);
    if ($('#privacy').length > 0) {
        if ($(window).width() > 400) {
            $('#privacy').find('#scrollbar1').css('width', '740');
            $('#privacy').find('#scrollbar1 .viewport').css('width', '715');
            $('#privacy').find('#scrollbar1').tinyscrollbar();
        }
        else {
            $('#privacy').removeClass('alerta').width('320px');
            $('#privacy').find('#scrollbar1').css('width', '320');
            $('#privacy').find('#scrollbar1 .viewport').css('height', 'auto').css('width', '310');
            $('#privacy').find('#scrollbar1').tinyscrollbar();
            $('#privacy').find('#scrollbar1 .overview').css('position', 'relative');
            $('#privacy').find('#scrollbar1 .scrollbar').hide();
        }
    }
    $('.media-item').click(changeMediaItem);

    if ($('#idea-text').length) {
        $('#idea-text').bind('input propertychange keyup', function () {
            validLong(this, $('#idea-text').attr('data-length') * 1, $('#idea-text-counter'));
        });
        $('#idea-text').hashtags();
    }

    $('#Description').bind('input propertychange keyup', function () {
        validLong(this, 250, $('#user-text-counter'));
    });
    $('#Message').bind('input propertychange keyup', function () {
        validLong(this, 500, $('#user-text-counter'));
        resizeColorBox($(document).width(), $(document).height())
    });

    $('#contacto-aceptar').click(function (event) {
        validateContactForm();
    });
    $('.alerta.registro .alerta-titulo .alerta-cerrar').click(function (event) {
        $('.alerta-cerrar').click(closeColorbox);
    });
    $('#system-alert .alerta-titulo .alerta-cerrar').click(function (event) {
        $('.alerta-cerrar').click(closeColorbox);
        $('.alerta-aceptar').click(closeColorbox);
    });
    $('#create-idea').click(function (event) {
        createIdea(function () {
            if ($('#idea-image-name').val().length > 0) {
                $('#idea-text').animate({ width: '+=111' }, 1000);
                $('.highlighter').animate({ width: '+=111' }, 1000);
                $('#idea-image-file-wraper').animate({ width: '-=100' }, 1000).hide();
            }
            $('#idea-text').val('').keyup();
            $('#idea-text-counter').html('350');
            $('#idea-image-file').attr('src', '').hide();
            $('#idea-image-name').val('');
            $('#idea-video-url').val('');
            $('.participa-comentarios h1').html($('.participa-comentarios h1').html() * 1 + 1);
            ideasPagin(1, 6, $('#idea-content-id').val(), 2, function ($html) {
                if ($('#no-ideas-sorry').length > 0) {
                    $('#no-ideas-sorry').replaceWith($html);
                }
                else if ($html.length) {
                    $('#search-list').prepend($html[0]).masonry('prepended', $html[0]).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                    masonryInstance = $('#search-list');

                    $($html[0]).find('div[data-friendly] img').load(function () {
                        if (masonryInstance) {
                            masonryInstance.masonry();
                        }
                    });
                }
                renderIdeas();
            });
        });
    });
    $('#create-idea-espanol').click(function (event) {
        createIdeaEspanol(function () {
            if ($('#idea-image-name').val().length > 0) {
                $('#idea-text').animate({ width: '+=111' }, 1000);
                $('.highlighter').animate({ width: '+=111' }, 1000);
                $('#idea-image-file-wraper').animate({ width: '-=100' }, 1000).hide();
            }
            $('#idea-text').val('').keyup();
            $('#idea-text-counter').html('350');
            $('#idea-image-file').attr('src', '').hide();
            $('#idea-image-name').val('');
            $('#idea-video-url').val('');
            $('.participa-comentarios h1').html($('.participa-comentarios h1').html() * 1 + 1);
            ideasPagin(1, 6, $('#idea-content-id').val(), 2, function ($html) {
                if ($('#no-ideas-sorry').length > 0) {
                    $('#no-ideas-sorry').replaceWith($html);
                }
                else if ($html.length) {
                    $('#search-list').prepend($html[0]).masonry('prepended', $html[0]).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                    masonryInstance = $('#search-list');

                    $($html[0]).find('div[data-friendly] img').load(function () {
                        if (masonryInstance) {
                            masonryInstance.masonry();
                        }
                    });
                }
                renderIdeas();
            });
        });
    });
    $('#create-idea-ingles').click(function (event) {
        createIdeaIngles(function () {
            if ($('#idea-image-name').val().length > 0) {
                $('#idea-text').animate({ width: '+=111' }, 1000);
                $('.highlighter').animate({ width: '+=111' }, 1000);
                $('#idea-image-file-wraper').animate({ width: '-=100' }, 1000).hide();
            }
            $('#idea-text').val('').keyup();
            $('#idea-text-counter').html('350');
            $('#idea-image-file').attr('src', '').hide();
            $('#idea-image-name').val('');
            $('#idea-video-url').val('');
            $('.participa-comentarios h1').html($('.participa-comentarios h1').html() * 1 + 1);
            ideasPagin(1, 6, $('#idea-content-id').val(), 2, function ($html) {
                if ($('#no-ideas-sorry').length > 0) {
                    $('#no-ideas-sorry').replaceWith($html);
                }
                else if ($html.length) {
                    $('#search-list').prepend($html[0]).masonry('prepended', $html[0]).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                    masonryInstance = $('#search-list');

                    $($html[0]).find('div[data-friendly] img').load(function () {
                        if (masonryInstance) {
                            masonryInstance.masonry();
                        }
                    });
                }
                renderIdeas();
            });
        });
    });
    renderIdeas();
    renderMyIdeas();
    $('.video').click(function () {
        $('#idea-video-url').show();
        if ($('#idea-image-name').val() != '') {
            $('#idea-image-delete').click();
        }
    });
    /*
    if ($('#map-canvas').length > 0) {
        initializeMap();
    }
    */
    // init challenges pagin
    if ($('.seccion-reto').length > 0) {
        clearTimeout(paginCicle);
        paginCicle = setTimeout(function () {
            scrollPagin('.seccion-reto', 100, null, function () {
                challengesNextPage(1, 6, true);
            })
        }, 100);
    }
    // init questions pagin
    if ($('.seccion-pregunta').length > 0) {
        clearTimeout(paginCicle);
        paginCicle = setTimeout(function () {
            scrollPagin('.seccion-pregunta', 100, null, function () {
                questionsNextPage(1, 6, true, 2);
            })
        }, 100);
    }

    $('.seccion-filtros-block a').click(function (event) {
        clearTimeout(paginCicle);
        $('.seccion-filtros-block a').removeAttr('id');
        $(this).attr('id', 'default-filter');
        var filter = $(this).attr('data-id');
        $('#questions-list-container').empty();
        var active = null;
        //var active = false;
        //if ($('.seccion-filtro-act').html().toUpperCase() == "ACTIVAS") {
        //    active = true;
        //}
        //else if ($('.seccion-filtro-act').html().toUpperCase() == "FINALIZADAS") {
        //    active = false;
        //}
        questionsPagin(1, 6, active, filter, function (html) {
            $('#questions-list-container').html(html);
        });
    });
    // init ideas pagin
    if ($('.ideas-item').length > 0) {

        clearTimeout(paginCicle);
        paginCicle = setTimeout(function () {
            scrollPagin('.ideas-item', 100, null, function () {
                ideasNextPage(1, 6, $('#idea-content-id').val(), 2)
            })
        }, 100);
    }

    $('.filtros a').click(function (event) {
        if ($('#search-type').length) {
            $('#search-type').val('2');
            $('#search-text').val('');
        }
        clearTimeout(paginCicle);
        $('.filtros a').removeAttr('id');
        $(this).attr('id', 'default-filter');
        var filter = $(this).attr('data-id');
        masonryInstance.masonry('destroy');
        $('#search-list').empty();
        ideasPagin(1, 6, $('#idea-content-id').val(), filter, function ($html) {
            if ($('#no-ideas-sorry').length > 0) {
                $('#no-ideas-sorry').replaceWith($html);
            }
            else {
                $('#search-list').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#search-list');
            }
            renderIdeas();
        });
    });
    // init ideas perfil pagin
    if ($('#my-ideas-perfil').length > 0) {
        $('#my-conversations').click(function (event) {
            $('.perfil-filtros').hide();
            $('#my-conversations').addClass('perfil-boton-selected');
            $('#my-ideas').removeClass('perfil-boton-selected');
            clearTimeout(paginCicle);
            masonryInstance.masonry('destroy');
            $('#my-ideas-perfil').empty();
            myIdeasPagin(1, 6, $('#user-id').val(), 1, '/perfil/misconversaciones', function ($html) {
                $('#my-ideas-perfil').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#my-ideas-perfil');
                renderMyIdeas();
            });
        });
        $('#my-ideas').click(function (event) {
            $('.perfil-filtros').show();
            $('#my-conversations').removeClass('perfil-boton-selected');
            $('#my-ideas').addClass('perfil-boton-selected');
            $('#profile-commented').removeClass('perfil-boton-selected');
            $('#profile-recent').addClass('perfil-boton-selected');
            clearTimeout(paginCicle);
            masonryInstance.masonry('destroy');
            $('#my-ideas-perfil').empty();
            myIdeasPagin(1, 6, $('#user-id').val(), 1, '/perfil/misideas', function ($html) {
                $('#my-ideas-perfil').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#my-ideas-perfil');
                renderMyIdeas();
            });
        });
        $('#profile-recent').click(function (event) {
            $('#profile-commented').removeClass('perfil-boton-selected');
            $('#profile-recent').addClass('perfil-boton-selected');
            clearTimeout(paginCicle);
            masonryInstance.masonry('destroy');
            $('#my-ideas-perfil').empty();
            myIdeasPagin(1, 6, $('#user-id').val(), 1, '/perfil/misideas', function ($html) {
                $('#my-ideas-perfil').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#my-ideas-perfil');
                renderMyIdeas();
            });
        });
        $('#profile-commented').click(function (event) {
            $('#profile-commented').addClass('perfil-boton-selected');
            $('#profile-recent').removeClass('perfil-boton-selected');
            clearTimeout(paginCicle);
            masonryInstance.masonry('destroy');
            $('#my-ideas-perfil').empty();
            myIdeasPagin(1, 6, $('#user-id').val(), 2, '/perfil/misideas', function ($html) {
                $('#my-ideas-perfil').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#my-ideas-perfil');
                renderMyIdeas();
            });
        });
        clearTimeout(paginCicle);
        $('#my-ideas-perfil').masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
        masonryInstance = $('#my-ideas-perfil');
        paginCicle = setTimeout(function () {
            scrollPagin('.ideas-item', 200, null, function () {
                myIdeasNextPage(1, 6, $('#user-id').val(), 1, '/perfil/misideas')
            })
        }, 100);
    }
    // init level3 comment pagin
    imageMargin('.reto-usuario-block img', 40, 40);
    if ($('.reto-comentarios img').length > 0) {
        renderCommentsLevel3();
        clearTimeout(paginLayerCicle);
        paginLayerCicle = setTimeout(function () {
            scrollPagin('.reto-comentarios img', 100, null, function () {
                if ($('#content-id').length && $('#content-id').val().length > 0) {
                    commentsBlogNextPage(1, 6, $('#content-id').val(), '_Nivel3IdeaCommentsList', null, '.reto-comentarios img');
                }
                else if ($('#filter-idea-id').length && $('#filter-idea-id').val().length > 0) {
                    commentsNextPage(1, 6, $('#filter-idea-id').val(), '_Nivel3IdeaCommentsList', null, '.reto-comentarios img');
                }
            });
        }, 100);
    }

    // init blog 
    renderAboutUs();

    if ($('.vimeo-image').length > 0) {
        $('.vimeo-image').each(function (index) {
            vimeoThumbnail($(this));
        })
    }

    // init finished content
    $('#content-ideas').click(function (event) {
        $('#search-list').show();
        $('#blog-list-container').hide();
    });

    $('#content-blog').click(function (event) {
        $('#search-list').hide();
        $('#blog-list-container').show();
    });

    if ($('.blog-item').length > 0) {
        clearTimeout(paginCicle);
        paginCicle = setTimeout(function () {
            scrollPagin('.blog-item', 100, null, function () {
                contentBlogNextPage(1, 6, $('#idea-content-id').val());
            })
        }, 100);
    }

    $('#flecha-derecha').click(function () {
        bannerHomeDerecha();
    });

    $('#flecha-izquierda').click(function () {
        if (!bannerAnimation && $('[data-banner]').length > 1) {
            removeTimeoutBanner();
            bannerAnimation = true;
            if (!bannerAnimationPan) {
                $('#flecha-izquierda-pan').click();
            }

            $('[data-banner=' + bannerPosition + ']').hide();
            bannerPosition--;
            if ($('[data-banner=' + bannerPosition + ']').length == 0) {
                bannerPosition = $('[data-banner]').length;
            }

            if ($('[data-banner=' + bannerPosition + ']').attr('data-video') == 'true') {
                $('#banner-link-2').html('<div id="player-holder"></div>');
                loadVideo($('[data-banner=' + bannerPosition + ']').attr('data-video-id'));
                $('#banner-link-2').removeAttr('href');
            }
            else {
                $('#banner-link-2').html('<img src="' + path + $('[data-banner=' + bannerPosition + ']').attr('data-image') + '" alt="banner" />');
                $('#banner-link-2').attr('href', $('[data-banner=' + bannerPosition + ']').attr('data-link'));
            }

            $('#banner-link-2').css('left', '-100%');
            $('#banner-link').animate({ left: '+=100%' }, 1000, function () { });

            $('#banner-link-2').animate({ left: '+=100%' }, 1000, function () {
                $('#banner-link-2').attr('id', 'banner-link-temp');
                $('#banner-link').attr('id', 'banner-link-2');
                $('#banner-link-temp').attr('id', 'banner-link');

                $('#banner-link-2').css('left', '-100%');
                $('#banner-link-2').empty();
                bannerAnimation = false;
                setTimeoutBanner();
            });

            $('[data-banner=' + bannerPosition + ']').show();
        }
    });

    $('#flecha-derecha-pan').click(function () {
        bannerHomeDerechaPan();
    });

    $('#flecha-izquierda-pan').click(function () {
        if (!bannerAnimationPan && $('[data-banner]').length > 1) {
            bannerAnimationPan = true;
            if (!bannerAnimation) {
                $('#flecha-izquierda').click();
            }

            $('[data-banner-pan=' + bannerPositionPan + ']').hide();
            bannerPositionPan--;
            if ($('[data-banner-pan=' + bannerPositionPan + ']').length == 0) {
                bannerPositionPan = $('[data-banner-pan]').length;
            }

            if ($('[data-banner-pan=' + bannerPositionPan + ']').attr('data-video') == 'true') {
                $('#banner-link-pan-2').html('<div id="player-holder2"></div>');
                loadVideo2($('[data-banner-pan=' + bannerPositionPan + ']').attr('data-video-id'));
                $('#banner-link-pan-2').removeAttr('href');
            }
            else {
                $('#banner-link-pan-2').html('<img src="' + path + $('[data-banner=' + bannerPositionPan + ']').attr('data-image') + '" alt="banner" />');
                $('#banner-link-pan-2').attr('href', $('[data-banner=' + bannerPositionPan + ']').attr('data-link'));
            }

            $('#banner-link-pan-2').css('left', '-768px');
            $('#banner-link-pan').animate({ left: '+=768' }, 1000, function () { });

            $('#banner-link-pan-2').animate({ left: '+=768' }, 1000, function () {

                $('#banner-link-pan-2').attr('id', 'banner-link-pan-temp');
                $('#banner-link-pan').attr('id', 'banner-link-pan-2');
                $('#banner-link-pan-temp').attr('id', 'banner-link-pan');

                $('#banner-link-pan-2').css('left', '-768px');
                bannerAnimationPan = false;
            });
            $('[data-banner-pan=' + bannerPositionPan + ']').show();
        }
    });

    if ($('[data-banner]').length == 1) {
        $('#flecha-derecha').hide();
        $('#flecha-izquierda').hide();
        $('#flecha-derecha-pan').hide();
        $('#flecha-izquierda-pan').hide();
    }

    if (readCookie('vs') == 'true') {
        eraseCookie('vs');
    }

    setTimeoutBanner();
    checkMasonry();
});
// END EVENTS, INIT & VALIDATORS

// REGULAR

function bannerHomeDerechaPan() {
    if (!bannerAnimationPan && $('[data-banner]').length > 1) {
        bannerAnimationPan = true;
        if (!bannerAnimation) {
            bannerHomeDerecha();
        }

        $('[data-banner-pan=' + bannerPositionPan + ']').hide();
        bannerPositionPan++;
        if ($('[data-banner-pan=' + bannerPositionPan + ']').length == 0) {
            bannerPositionPan = 1;
        }

        if ($('[data-banner-pan=' + bannerPositionPan + ']').attr('data-video') == 'true') {
            $('#banner-link-pan-2').html('<div id="player-holder2"></div>');
            loadVideo2($('[data-banner-pan=' + bannerPositionPan + ']').attr('data-video-id'));
            $('#banner-link-pan-2').removeAttr('href');
        }
        else {
            $('#banner-link-pan-2').html('<img src="' + path + $('[data-banner=' + bannerPositionPan + ']').attr('data-image') + '" alt="banner" />');
            $('#banner-link-pan-2').attr('href', $('[data-banner=' + bannerPositionPan + ']').attr('data-link'));
        }

        $('#banner-link-pan-2').css('left', '768px');
        $('#banner-link-pan').animate({ left: '-=768' }, 1000, function () { });

        $('#banner-link-pan-2').animate({ left: '-=768' }, 1000, function () {

            $('#banner-link-pan-2').attr('id', 'banner-link-pan-temp');
            $('#banner-link-pan').attr('id', 'banner-link-pan-2');
            $('#banner-link-pan-temp').attr('id', 'banner-link-pan');

            $('#banner-link-pan-2').css('left', '768px');
            $('#banner-link-pan-2').empty();
            bannerAnimationPan = false;
        });

        $('[data-banner-pan=' + bannerPositionPan + ']').show();
    }
}

function bannerHomeDerecha() {
    if (!bannerAnimation && $('[data-banner]').length > 1) {
        removeTimeoutBanner();
        bannerAnimation = true;
        if (!bannerAnimationPan) {
            bannerHomeDerechaPan();
        }

        $('[data-banner=' + bannerPosition + ']').hide();
        bannerPosition++;
        if ($('[data-banner=' + bannerPosition + ']').length == 0) {
            bannerPosition = 1;
        }

        if ($('[data-banner=' + bannerPosition + ']').attr('data-video') == 'true') {
            $('#banner-link-2').html('<div id="player-holder"></div>');
            loadVideo($('[data-banner=' + bannerPosition + ']').attr('data-video-id'));
            $('#banner-link-2').removeAttr('href');
        }
        else {
            $('#banner-link-2').html('<img src="' + path + $('[data-banner=' + bannerPosition + ']').attr('data-image') + '" alt="banner" />');
            $('#banner-link-2').attr('href', $('[data-banner=' + bannerPosition + ']').attr('data-link'));
        }

        $('#banner-link-2').css('left', '100%');
        $('#banner-link').animate({ left: '-=100%' }, 1000, function () { });

        $('#banner-link-2').animate({ left: '-=100%' }, 1000, function () {

            $('#banner-link-2').attr('id', 'banner-link-temp');
            $('#banner-link').attr('id', 'banner-link-2');
            $('#banner-link-temp').attr('id', 'banner-link');

            $('#banner-link-2').css('left', '100%');
            $('#banner-link-2').empty();
            bannerAnimation = false;
            setTimeoutBanner();
        });

        $('[data-banner=' + bannerPosition + ']').show();
    }
}

function renderPlaceholder() {
    $.each($('[placeholder]'), function (index, obj) {
        if ($(obj).attr('data-placeholder') != 'true') {
            $(obj).attr('data-placeholder', 'true');
        }
    });
}

function clearFileError(id) {
    $('#error-file[data-id="' + id + '"]').hide();
}

function checkIframeImageLoaded(id, errorId) {
    if (!iFrameImageLoaded) {
        if (errorId && $('#' + id + '[data-id="' + errorId + '"]').length) {
            $('#' + id + '[data-id="' + errorId + '"]').attr('src', $('#' + id + '[data-id="' + errorId + '"]').attr('src'));
        }
        else if ($('#' + id).length) {
            $('#' + id).attr('src', $('#' + id).attr('src'));
        }

        if (errorId) {
            $('#error-file[data-id="' + errorId + '"]').show();
        }
        else {
            $('#error-file').show();
        }
    }
}

function search() {
    var contentId = null;
    if ($('#search-content').length) {
        $('.result-header').hide();
        contentId = $('#search-content').attr('data-id') * 1;
    }
    var text = $('#search-text').val().trim();
    var type = $('#search-type').val();

    var filter = null;
    if ($('#send-search').attr('data-small') == 'true') {
        filter = 1;
    }

    clearTimeout(paginCicle);
    switch (type) {
        case '1':
            userPaging(0, contentId, text, filter);
            break;
        case '2':
            ideasPaging(0, contentId, text);
            break;
        case '3':
            pulsesPagin(0, text);
            break;
    }

    $('#search-text').val(text);
}

function searchcasosexito() {
    var contentId = null;
    if ($('#search-content').length) {
        $('.result-header').hide();
        contentId = $('#search-content').attr('data-id') * 1;
    }
    var text = $('#search-text').val().trim();
    var type = $('#search-type').val();

    var filter = null;
    if ($('#send-searchcasosexito').attr('data-small') == 'true') {
        filter = 1;
    }

    clearTimeout(paginCicle);
    switch (type) {
        case '1':
            userPaging(0, contentId, text, filter);
            break;
        case '2':
            ideasPaging(0, contentId, text);
            break;
        case '3':
            pulsesPagin(0, text);
            break;
    }

    $('#search-text').val(text);
}

function filterCommunity(event) {
    var $target = $(this);
    var filter = $target.attr('data-id');
    clearTimeout(paginCicle);
    $('.mm2-item-organizer').removeClass('selected');
    $target.addClass('selected');
    $('#search-list').empty();
    userPaging(0, null, null, filter);
}

function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

function setCaretToPos(input, pos) {
    setTimeout(function () {
        setSelectionRange(input, pos, pos);
    }, 10);
}

function cursorCurrentWord($target, replace) {
    var stopStart = [' ', '\n', '\r', '\t']
    var stopEnd = [' ', '\n', '\r', '\t', '#']

    var text = $target.val();
    var start = $target[0].selectionStart - 1;
    var end = $target[0].selectionEnd;
    while (start > 0) {
        if (stopStart.indexOf(text[start]) == -1) {
            --start;
        }
        else {
            break;
        }
    };

    while (end < text.length) {
        if (stopEnd.indexOf(text[end]) == -1) {
            ++end;
        }
        else {
            break;
        }
    }
    var currentWord = text.substr(start, end - start);

    if (replace) {
        text = text.replaceBetween(start, end, replace);
        $target.val(text);
        setCaretToPos($target[0], start + replace.length + 2);
    }

    return currentWord.trim();
}

function noNumbers(event) {
    if (event.which >= 48 && event.which <= 57) {
        return false;
    }
    else {
        return true;
    }
}

function onlyNumbers(event) {
    if ((event.which >= 48 && event.which <= 57) || event.which == 0 || event.which == 8) {
        return true;
    }
    else {
        return false;
    }
}

function setTimeoutBanner() {
    bannerTimeout = setTimeout(function () {
        bannerHomeDerecha();
    }, 5000);
}

function removeTimeoutBanner() {
    clearTimeout(bannerTimeout);
}

function getShortUrl(url, callback) {
    $.getJSON(
        "http://api.bitly.com/v3/shorten?callback=?",
        {
            'format': 'json',
            'apiKey': 'R_bbac074b12c74c12a6c873fabd59e468',
            'login': 'o_4dg511nbpe',
            'longUrl': url
        },
        function (response) {
            if (callback) {
                callback(response.data.url);
            }
        }
    );
}

function validLongComments() {
    $('.idea-comentario textarea[data-event-bind=false]').bind('input propertychange keyup', function () {
        validLong(this, $(this).attr('data-length') * 1, $('#comment-idea-counter-' + $(this).attr('data-id')));
    }).attr('data-event-bind', 'true');

    $('.nosotros-contenido-comentario-txt textarea[data-event-bind=false]').bind('input propertychange keyup', function () {
        validLong(this, $(this).attr('data-length') * 1, $('#comment-blog-counter-' + $(this).attr('data-id')));
    }).attr('data-event-bind', 'true');

    $('.reto-comentarios textarea[data-event-bind=false]').bind('input propertychange keyup', function () {
        validLong(this, $(this).attr('data-length') * 1, $('#comment-blog-counter-' + $(this).attr('data-id')));
    }).attr('data-event-bind', 'true');

    $('.generic-comment textarea[data-event-bind]').unbind('input propertychange keyup').bind('input propertychange keyup', function () {
        validLong(this, $(this).attr('data-length') * 1, $('#comment-counter-' + $(this).attr('data-id')));
    }).attr('data-event-bind', 'true');
}

//function brokenImage() {
//    //try {
//    //    $("img").each(function () {
//    //        if (this.complete) {
//    //            $(this).attr("src", $(this).attr("src"));
//    //        }
//    //    }).error(function () {
//    //        var $target = $(this);
//    //        if ($target.width() <= 58 && $target.height() <= 58 && $target.width() > 0 && $target.height() > 0) {
//    //            $target.unbind('error').attr('src', path + '/files/imagesuser/default.png');
//    //        }
//    //    });
//    //}
//    //catch (ex) { }
//}

function replaceTextURLWithHTMLLinks(text) {
    var exp = "/(https?:[/][/][-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?)/gi";
    text = text.replace(exp, '<a href="$1" target="_blank">$1</a>');

    text = styleUpHashTags(text);

    return text;
}

function styleUpHashTags(text) {
    var exp = /(#\w+)/gi;
    text = text.replace(exp, '<a>$1</a>');
    return text;
}

function validLong(obj, num_caracteres_permitidos, spn, edit) {
    var num_caracteres = $(obj).val().length;

    if (spn != null) {
        spn.html(num_caracteres_permitidos - num_caracteres);
    }

    if (num_caracteres <= num_caracteres_permitidos) {
        $(obj).attr('valid-long', 'true');
        spn.css('color', '');
        if (edit) {
            var $parent = $(obj).parent().parent();
            $parent.find('.update-block div').css('background-color', '');
            $parent.find('.update-block a').css('background-color', '');
        }
        else {
            $('.participa-block div').css('background-color', '');
            $('.participa-block a').css('background-color', '');
        }
    }
    else {
        $(obj).attr('valid-long', 'false');
        spn.css('color', 'red');
        if (edit) {
            var $parent = $(obj).parent().parent();
            $parent.find('.update-block div').css('background-color', '#777777');
            $parent.find('.update-block a').css('background-color', '#A7A7A7');
        }
        else {
            $('.participa-block div').css('background-color', '#777777');
            $('.participa-block a').css('background-color', '#A7A7A7');
        }
    }
};

function showAdmin(event) {
    event.stopPropagation();
    event.preventDefault();
    var $element = $(this);
    var $parent = $element.parent();
    var $target = $parent.find('#admintblock');

    $target.show();

    if ($target.offset().left + $target.outerWidth() > window.innerWidth) {
        $target.css('left', ($target.offset().left + $target.outerWidth()) * -1)
    }
    else if ($target.offset().left < 0) {
        $target.css('left', ($target.offset().left + $target.outerWidth()))
    }

    if (($target.offset().top - $(window).scrollTop()) + $target.outerHeight() > window.innerHeight) {
        $target.css('top', (($target.offset().top - $(window).scrollTop()) + $target.outerHeight() - window.innerHeight) * -1)
    }
}

function hideAdmin(event) {
    event.stopPropagation();
    var $element = $(this);
    var $target = $element.parent().parent();
    $target.hide();
}

function showTermsRegistry() {
    if ($(window).width() > 768) {
        $('#system-terms').show();
        $('#system-terms').parent().show();
        $('#system-terms').parent().css('position', 'absolute').css('top', '0').css('left', '0').css('width', '760');
        $('#system-terms').find('#scrollbar1').css('width', '740');
        $('#system-terms').find('#scrollbar1 .viewport').css('width', '715');
        $('#system-terms').find('#scrollbar1').tinyscrollbar();
        $('#system-terms').find('#terminos-aceptar').unbind('click');
        $('#system-terms').find('#terminos-aceptar').click(hideTermsRegistry);
        $('#system-terms').find('.alerta-cerrar').unbind('click');
        $('#system-terms').find('.alerta-cerrar').click(hideTermsRegistry);
    }
    else {
        openTab(path + '/legal/terms');
    }
}

function hideTermsRegistry() {
    $('#system-terms').hide();
    $('#system-terms').find('.alerta-aceptar').show();
    $('#system-terms').parent().hide();
}

function showPrivacyRegistry() {
    if ($(window).width() > 768) {
        $('#system-privacy').show();
        $('#system-privacy').parent().show();
        $('#system-privacy').parent().css('position', 'absolute').css('top', '0').css('left', '0').css('width', '760');
        $('#system-privacy').find('#scrollbar1').css('width', '740');
        $('#system-privacy').find('#scrollbar1 .viewport').css('width', '715');
        $('#system-privacy').find('#scrollbar1').tinyscrollbar();
        $('#system-privacy').find('#terminos-aceptar').unbind('click');
        $('#system-privacy').find('#terminos-aceptar').click(hidePrivacyRegistry);
        $('#system-privacy').find('.alerta-cerrar').unbind('click');
        $('#system-privacy').find('.alerta-cerrar').click(hidePrivacyRegistry);
    }
    else {
        openTab(path + '/legal/privacy');
    }
}

function hidePrivacyRegistry() {
    $('#system-privacy').hide();
    $('#system-privacy').find('.alerta-aceptar').show();
    $('#system-privacy').parent().hide();
}

function shareLink(event, $share, $element, $target) {
    var $shareTarget = $share.parent();
    var network = $share.attr('data-id');
    var ideaId = $shareTarget.attr('data-idea-id');
    var name = $shareTarget.attr('data-name');
    var link = $shareTarget.attr('data-link');
    var shortLink = $shareTarget.attr('data-short-link');
    var picture = $shareTarget.attr('data-picture');
    var caption = $shareTarget.attr('data-caption');
    var description = $shareTarget.attr('data-description');
    var descriptionFB = $shareTarget.attr('data-description-fb');
    var descriptionTW = $shareTarget.attr('data-description-tw');
    var email = $shareTarget.attr('data-email');
    switch (network) {
        case 'facebook':
            if (name.length > 80) {
                name = name.substring(70, name.indexOf(' ') - 80);
                name = name + '...';
            }
            facebookShare(name, link, picture, caption, descriptionFB, function () {
                if (ideaId) {
                    ideaShare(ideaId, 'facebook');
                }
                $element.trigger('click');
                //hideCompartir(null, $element, $target);
            });
            break;
        case 'twitter':
            popupWindow('https://twitter.com/share?url=' + encodeURIComponent(shortLink) + '&text=' + encodeURIComponent(descriptionTW), name, 640, 450);
            if (ideaId) {
                ideaShare(ideaId, 'twitter');
            }
            $element.trigger('click');
            //hideCompartir(null, $element, $target);
            break;
        case 'google':
            popupWindow('https://plus.google.com/share?url=' + link, name, 500, 450);
            $element.trigger('click');
            if (ideaId) {
                ideaShare(ideaId, 'google');
            }
            //hideCompartir(null, $element, $target);
            break;
        case 'linkedin':
            var linkedin_window = IN.UI.Share().params({
                url: link
            }).place();
            linkedin_window.success(function () {
                $element.trigger('click');
                if (ideaId) {
                    ideaShare(ideaId, 'linkedin');
                }
                //hideCompartir(null, $element, $target);
            });
            break;
        case 'correo':
            redirect('mailto:?subject=' + name + '&body=' + description + ' ' + link);
            $element.trigger('click');
            //hideCompartir(null, $element, $target);            
            break;
    }
}

function showCompartir() {
    var $element = $(this);
    $element.unbind('click');
    var $target = $element.parent().find('#compartirblock');
    $target.show();
    $target.animate({ height: '200px', opacity: '0.5' }, 500);
    $target.animate({ height: '190px', opacity: '1' }, 200, function () {
        var scroll = $('#scrollbar2').length > 0 ? '#scrollbar2' : '#scrollbar';
        if ($(scroll).length > 0) {
            $(scroll).tinyscrollbar_update('relative');
        }
    });
    $target.find('#compartirbc').unbind('click');
    $target.find('#compartirbc').click(function (event) {
        hideCompartir(event, $element, $target);
    });
    $element.unbind('click');
    $element.click(function (event) {
        hideCompartir(event, $element, $target);
    });

    $target.find('.compartir-div-item').unbind('click');
    $target.find('.compartir-div-item').click(function (event) {
        shareLink(event, $(this), $element, $target);
    });

    if ($target.find('.compartir-data').attr('data-short-link').length == 0) {
        getShortUrl($target.find('.compartir-data').attr('data-link'), function (shortURL) {
            $target.find('.compartir-data').attr('data-short-link', shortURL);
        });
    }
}

function hideCompartir(event, $element, $target) {
    $target.animate({ height: '200px', opacity: '0.5' }, 200);
    $target.animate({ height: '0px', opacity: '0' }, 500, function () { $target.hide(); });
    event.stopPropagation();
    $element.unbind('click');
    $element.click(showCompartir);
}

function showSocialLink() {
    var object = this;
    $(object).find('h2').hide();
    if (!$(object).find('input').is(':visible')) {
        $(object).find('input').val($(object).attr('data-text'));
    }
    $(object).find('input').show().focus();
    $(object).find('.blog-enlaces-plus').show();
    //$(object).find('input').unbind('click');
    //$(object).find('input').click(function () {
    //    //if ($(object).find('input').val().length > 0) {
    //        updateSocialLink(object);
    //    //}
    //});
    $(object).find('input').unbind('keyup').keyup(function (event) {
        if (event.keyCode == 13 /*&& $(object).find('input').val().length > 0*/) {
            updateSocialLink(object);
        }
    });

    $(object).find('.blog-enlaces-plus').unbind('click').click(function () {
        updateSocialLink(object);
    });
}

function commentarioEnterKey(event) {
    if (event.keyCode == 13 || event.originalEvent.type == 'click') {
        var input = null;
        if (event.originalEvent.type == 'click') {
            input = $(this).parent().find('textarea');
        }
        else {
            input = $(this);
        }

        if (input.attr('valid-long') != 'true' && input.attr('data-length')) {
            systemAlert('¡Uups!', 'Tu comentario debe tener máximo ' + input.attr('data-length') + ' caracteres.');
            return;
        }

        var id = input.attr('data-id') * 1;
        var size = input.attr('data-size') * 1;
        var container = input.attr('data-container');
        var selector = input.attr('data-selector');
        var tinyScroll = input.attr('data-scroll');
        var paginView = input.attr('data-view');

        createComment(id, input.val(), function (html) {
            input.val('').trigger('propertychange');
            commentsPagin(1, size, id, paginView, tinyScroll, selector, function (html) {
                $(container).html(html);
                //$('#search-list').masonry({ columnWidth: ideaColumnWidth });
                //masonryInstance = $('#search-list');
                imageMargin(selector, 32, 32);
                renderAdmin();
                renderIdeas();
                if (masonryInstance) {
                    masonryInstance.masonry();
                }
                if ($(tinyScroll).length) {
                    $(tinyScroll).tinyscrollbar_update('relative');
                }
            });
        });

        event.preventDefault();
        return false;
    }
}

function editCommentarioEnterKey(event, location, $this) {
    if (event.keyCode == 13 || event.originalEvent.type == 'click') {
        var input = null;
        if (event.originalEvent.type == 'click') {
            input = $this.parent().find('textarea');
        }
        else {
            input = $this;
        }
        if (input.attr('valid-long') != 'true' && input.attr('data-length')) {
            systemAlert('¡Uups!', 'Tu comentario debe tener máximo ' + input.attr('data-length') + ' caracteres.');
            return;
        }

        var id = input.attr('data-id') * 1;

        var targetClass = 'idea-comentario';
        var targetView = '';

        switch (location) {
            case 'layer-comments':
                targetClass = 'layer-comentario';
                targetView = '_LayerIdeaCommentsList';
                break;
            case 'layer-image-comments':
                targetClass = 'layer-comentario';
                targetView = '_LayerIdeaCommentsListImage';
                break;
            case 'index-comments':
                targetClass = 'reto-comentarios';
                targetView = '_Nivel3IdeaCommentsList';
                break;
        }

        updateComment(id, input.val(), function () {
            $.ajax({
                type: 'POST',
                dataType: 'html',
                url: path + '/contenido/comentario',
                data: { commentId: id, view: targetView }
            }).done(function (html) {
                html = html.trim();
                if (html.length) {
                    var $content = $(html);
                    var $target = $('.' + targetClass + '[data-id="' + id + '"]');
                    $target.replaceWith($content);
                    if (masonryInstance) {
                        masonryInstance.masonry();
                    }
                    renderAdmin();
                    renderIdeas();
                    if (location == 'about-comments') {
                        renderAboutUs();
                    }
                }
                else {

                }
            });
        });

        event.preventDefault();
        return false;
    }
}

function enterBanner() {
    $(this).stop();
    $(this).animate({ opacity: '0.6' }, 300);
}

function outBanner() {
    $(this).stop();
    $(this).animate({ opacity: '1' }, 100);
}

function showPan(event) {
    //var element = $(this);

    //$('.img5').css('background-image', 'url(' + $('.img5').parent().attr('data-image') + ')');

    //$("#home-images .img5").removeClass("img5");
    //$(event.target).animate({ opacity: '0.5' }, 0);
    //$(event.target).animate({ opacity: '1' }, 700);
    //$(event.target).addClass("img5");

    //switch (element.attr('data-id')) {
    //    case 'p-0':// seleccion multiple
    //        $('.img5').css('background-image', 'url(' + path + '/Resources/Images/MiMedellin/img4.jpg)');
    //        break;
    //    case 'p-1':// ubicacion
    //        $('.img5').css('background-image', 'url(' + path + '/Resources/Images/MiMedellin/img3.jpg)');
    //        break;
    //    case 'p-2':// abierta
    //        $('.img5').css('background-image', 'url(' + path + '/Resources/Images/MiMedellin/img5.jpg)');
    //        break;
    //    case 'r-0':// participacion
    //        $('.img5').css('background-image', 'url(' + path + '/Resources/Images/MiMedellin/img1.jpg)');
    //        break;
    //    case 'r-1'://reto
    //        $('.img5').css('background-image', 'url(' + path + '/Resources/Images/MiMedellin/img2.jpg)');
    //        break;
    //}
    //$("#Pan img").attr("src", $(event.target).attr("big-image"));
    //$('#Pan a').attr('href', $(event.target).attr("data-link"));
    //$("#Pan").stop();
    //$("#Pan").animate({ opacity: '0.5' }, 50);
    //$("#Pan").animate({ opacity: '1' }, 800);
    //$('.banner-info').hide();
    //$('.banner-infop').hide();
    //var contentId = $(event.target).attr('data-id');
    //$('#banner-info-' + contentId).show();
    //$('#banner-infop-' + contentId).show();
}

function commentsNextPage(pageIndex, pageSize, filterIdeaId, paginView, tinyScroll, selector, callback) {
    pageIndex = pageIndex + pageSize;
    commentsPagin(pageIndex, pageSize, filterIdeaId, paginView, tinyScroll, selector, callback);
}

function myIdeasNextPage(pageIndex, pageSize, usertId, filter, url, callback) {
    pageIndex = pageIndex + pageSize;
    myIdeasPagin(pageIndex, pageSize, usertId, filter, url, callback);
}

function ideasNextPage(pageIndex, pageSize, filterContentId, filterIdea, callback) {
    pageIndex = pageIndex + pageSize;
    ideasPagin(pageIndex, pageSize, filterContentId, filterIdea, callback);
}

function renderIdeasLinks() {
    $('.idea-text[data-render-url=true]').each(function () {
        var $target = $(this);
        $target.html(replaceTextURLWithHTMLLinks($target.html()));
        $target.attr('data-render-url', 'false');
    });
}

function renderVersusLinks() {
    $('.vs-contenido p[data-render-url=true]').each(function () {
        var $target = $(this);
        $target.html(replaceTextURLWithHTMLLinks($target.html()));
        $target.attr('data-render-url', 'false');
    });
}

function renderIdeas() {
    renderIdeasLinks();
    $('.idea-comentario textarea').unbind('keydown');
    $('.idea-comentario textarea').keydown(commentarioEnterKey);
    $('.comentar-coment').unbind('click');
    $('.comentar-coment').click(commentarioEnterKey);
    imageMargin('.ideas-item-usuario img', 40, 40);
    imageMargin('.idea-comentario-imagen img', 32, 32);
    $('.idea-click-area').unbind('click');
    $('.idea-click-area').click(function (event) {
        var id = $(this).attr('data-id');
        var friendly = $(this).attr('data-friendly');
        showIdea(id, friendly);
    });
    renderAdmin();
    renderIdeasActions();
    validLongComments();
    //    $('textarea').autosize();
    renderShowProfile();
}

function renderShowProfile() {
    if ($('[data-profile="true"]').length) {
        var $profiles = $('[data-profile="true"]');
        $profiles.attr('data-profile', 'false');
        $profiles.mouseenter(function () {
            var $this = $(this);
            var $target = $this.closest('.ideas-item');
            var $clone = $('#user-card').clone();
            $clone.removeAttr('id').attr('data-card', 'true');

            var $image = $target.find('div[data-friendly] img');
            if ($image.length) {
                $clone.css('top', $image.height() + 10);
            }

            var $video = $target.find('.idea-video-wraper');
            if ($video.length) {
                $clone.css('top', $video.height() + 10);
            }

            $clone.show();
            $target.append($clone);

            $.ajax({
                type: 'POST',
                dataType: 'html',
                url: path + '/perfil/usersmallprofile',
                data: {
                    userId: $this.attr('data-id')
                }
            }).done(function (html) {
                html = html.trim();
                if (html.length) {
                    ga('send', 'pageview', '/mostrat-tarjeta-perfil/' + $this.attr('data-id'));
                    var $html = $(html);
                    $html.attr('data-card', 'true').css('top', $clone.css('top')).addClass('mm2-card-people2').removeClass('mm2-card-people');
                    if ($clone.is(':visible')) {
                        $clone.remove();
                        $target.append($html);
                    }
                }
            });
        });

        $profiles.mouseout(function () {
            var $this = $(this);
            var $target = $this.closest('.ideas-item').find('[data-card="true"]');
            $target.remove();
        });
    }
}

function renderCommentsLevel3() {
    imageMargin('.reto-comentarios img', 40, 40);
    renderAdmin();
    renderIdeasActions();
    $('textarea').autosize();
}

function renderAdmin() {
    $('.admin-ico').unbind('click');
    $('.admin-ico').click(showAdmin);
    $('.item-div-a').unbind('click');
    $('.item-div-a').click(hideAdmin);
    $('.item-div-i').unbind('click');
    $('.item-div-i').click(function (event) { event.preventDefault(); event.stopPropagation(); adminAction($(this)); });
}

function renderTooptip() {
    //$('.content-tooltip').unbind('mouseover').bind('mouseover', showTitle);
    //$('.content-tooltip').unbind('mouseout').bind('mouseout', hideTitle);
}

function renderMyIdeas() {
    renderIdeas();
    /*
    renderIdeasLinks();
    $('.perfil-comentario textarea').unbind('keydown');
    $('.perfil-comentario textarea').keydown(commentarioEnterKey);
    $('.comentar-coment').unbind('click');
    $('.comentar-coment').click(commentarioEnterKey);
    imageMargin('.perfil-propuesta-usuario img', 40, 40);
    imageMargin('.perfil-comentario-img img', 32, 32);
    $('.idea-click-area').unbind('click');
    $('.idea-click-area').click(function (event) {
        var id = $(this).attr('data-id');
        var friendly = $(this).attr('data-friendly');
        showIdea(id, friendly);
    });
    renderAdmin();
    renderIdeasActions();
    validLongComments();
    $('textarea').autosize();
    */
}

function challengesNextPage(pageIndex, pageSize, filterActive, callback) {
    pageIndex = pageIndex + pageSize;
    challengesPagin(pageIndex, pageSize, filterActive, callback);
}

function questionsNextPage(pageIndex, pageSize, filterActive, filter, callback) {
    pageIndex = pageIndex + pageSize;
    questionsPagin(pageIndex, pageSize, filterActive, filter, callback);
}

function scrollPagin(selector, distance, tinyScroll, callback) {
    if (callback && $(selector).length > 0) {

        var element = $($(selector)[$(selector).length - 1])
        var bottomElement = 0;
        var bottomScroll = 0;

        if (tinyScroll) {
            bottomElement = (element.position().top * 1) - ($($(selector)[0]).position().top * 1);
            bottomScroll = ($(tinyScroll + ' .viewport .overview').css('top').replace('px', '') * -1) + ($(tinyScroll + ' .viewport').height() * 1) + distance;
        }
        else {
            bottomElement = (element.position().top * 1);
            bottomScroll = ($(document).scrollTop() * 1) + ($(window).height() * 1) + distance;
        }

        if (bottomElement < bottomScroll) {
            //alert('pagino');
            callback();
        }
        else {
            if (tinyScroll) {
                paginLayerCicle = setTimeout(function () { scrollPagin(selector, distance, tinyScroll, callback) }, 100);
            }
            else {
                paginCicle = setTimeout(function () { scrollPagin(selector, distance, tinyScroll, callback) }, 100);
            }
        }
    }
}

function showSectionFilter(event) {
    $("#activoSubmenu").animate({ height: '55px', opacity: '0.5' }, 300, function () {
        $("#activoSubmenu").animate({ height: '53px', opacity: '1' }, 200, function () { });
        $(".seccion-filtro").unbind('click');
        $(".seccion-filtro").click(hideSectionFilter);
    });
}

function hideSectionFilter(event) {
    $("#activoSubmenu").animate({ height: '55px', opacity: '0.5' }, 200, function () {
        $("#activoSubmenu").animate({ height: '0px', opacity: '0' }, 300)
        $(".seccion-filtro").unbind('click');
        $(".seccion-filtro").click(showSectionFilter);
    });
}

function sectionFilter(event) {
    var filter = $(event.target).html().toUpperCase();
    var clicked = false;
    if (filter == "ACTIVOS") {
        $('#activoSubmenu').css('width', '114px');
        challengesPagin(1, 6, true, function (html) {
            $('#challenges-list-container').html(html);
            animatebar();
        });
        clicked = true;
    }
    else if (filter == "FINALIZADOS") {
        $('#activoSubmenu').css('width', '148px');
        challengesPagin(1, 6, false, function (html) {
            $('#challenges-list-container').html(html);
            animatebar();
        });
        clicked = true;
    }
    else if (filter == "ACTIVAS") {
        $('#activoSubmenu').css('width', '148px');
        var questionFilter = $('#default-filter').attr('data-id');
        questionsPagin(1, 6, true, questionFilter, function (html) {
            $('#questions-list-container').html(html);
            animatebar();
        });
        clicked = true;
    }
    else if (filter == "FINALIZADAS") {
        $('#activoSubmenu').css('width', '148px');
        var questionFilter = $('#default-filter').attr('data-id');
        questionsPagin(1, 6, false, questionFilter, function (html) {
            $('#questions-list-container').html(html);
            animatebar();
        });
        clicked = true;
    }

    if (clicked) {
        clearTimeout(paginCicle);
        $('.seccion-filtro-act').html(filter);
        hideSectionFilter();
    }
}

function imageMargin(selector, width, height) {

    $(selector).one('load', function () {
        var image = $(this);
        //image.css('margin-left', (width - image.width()) / 2);
        //image.css('margin-top', (height - image.height()) / 2);
        image.show();
    }).each(function () {
        if (this.complete) {
            $(this).load();
        }
    });

}

function showRegistryUserImage(image) {
    $('#user-image-file').attr('src', path + '/resources/temporal/' + image);
    $('#imageName').val('~/files/imagesuser/' + image);
    imageMargin('#user-image-file', 58, 58);
}

function showEditableImage(image, id) {
    var $image = null;
    var $input = null;
    if (id) {
        $image = $('#editable-image[data-id="' + id + '"]');
        $input = $('#editableImage[data-id="' + id + '"]');
        $('#error-file[data-id="' + id + '"]').hide();
    }
    else {
        $image = $('#editable-image');
        $input = $('#editableImage');
        $('#error-file').hide();
    }

    $image.show().attr('src', path + '/resources/temporal/' + image);
    $input.val(image);

    setTimeout(function () {
        resizeColorBox($('#editable-box').width(), $('#editable-box').height());
    }, 100);
}

function clearEditableImage(id) {
    $('#editable-image[data-id="' + id + '"]').hide();
    $('#editableImage[data-id="' + id + '"]').val('');
}

function showCreateIdeaImage(image) {
    var $target = $('#create-idea-block');

    if ($('#idea-image-delete').is(':visible')) {
        $('#idea-image-delete').click();
    }

    $('#idea-text').animate({ width: '-=20' }, 0, function () {
        $target.find('#idea-image-file-wraper').show().animate({ width: '+=100' }, 1000, function () {
            //imageMargin('#idea-image-file', 100, 70);
            $('#idea-text').trigger('input.autosize');
        });
        $('#idea-text').animate({ width: '-=100' }, 1000);
    });
    $target.find('.highlighter').animate({ width: '-=20' }, 0, function () {
        $target.find('.highlighter').animate({ width: '-=100' }, 1000);
    });
    $('#idea-video-url').hide();
    $('#idea-image-file').attr('src', path + '/resources/temporal/ideas/100x70-' + image).show();
    $('#idea-image-name').val(image);
    $('#idea-image-delete').show();
    $('#idea-image-delete').unbind('click');
    $('#idea-image-delete').click(function (e) {
        $.post(path + 'Idea/DeleteIdeaImage/', { image: image }, function (data) {
            $('#idea-image-delete').hide();
            $('#idea-image-file').hide();
            $('#idea-image-name').val('');
        });
        $('#idea-text').animate({ width: '+=120' }, 1000);
        $('.highlighter').animate({ width: '+=120' }, 1000);
        $('#idea-image-file-wraper').animate({ width: '-=100' }, 1000, function () {
            $('#idea-text').trigger('input.autosize');
        }).hide();
    });
}

function showUpdateIdeaImage(ideaId, image) {
    var $target = $('.ideas-item[data-id="' + ideaId + '"][data-idea-card]');

    if ($target.find('#idea-image-delete').is(':visible')) {
        $target.find('#idea-image-delete').click();
    }

    $target.find('#idea-text-update').animate({ width: '-=20' }, 100, function () {
        $target.find('#idea-image-file-wraper').show().animate({ width: '+=100' }, 1000, function () {
            //$target.find('.idea-update-image-file').show();      
            $target.find('#idea-text-update').trigger('input.autosize')
        });
        $target.find('#idea-text-update').animate({ width: '-=91' }, 1000);
    });

    $target.find('.highlighter').animate({ width: '-=20' }, 1, function () {
        $target.find('.highlighter').animate({ width: '-=91' }, 1000);
    });

    $target.find('#idea-video-url').hide();
    $target.find('.idea-update-image-file').attr('src', path + '/resources/temporal/ideas/100x70-' + image);
    $target.find('#idea-image-name').val(image);
    $target.find('#idea-image-delete').show();
    $target.find('#idea-image-delete').unbind('click');
    $target.find('#idea-image-delete').click(function (e) {
        $.post(path + 'Idea/DeleteIdeaImage/', { image: image }, function (data) {
            $target.find('#idea-image-delete').hide();
            $target.find('.idea-update-image-file').hide();
            $target.find('#idea-image-name').val('');
        });
        $target.find('#idea-text-update').animate({ width: '+=111' }, 1000);
        $target.find('.highlighter').animate({ width: '+=111' }, 1000);
        $target.find('#idea-image-file-wraper').animate({ width: '-=100' }, 1000, function () {
            $target.find('#idea-text-update').trigger('input.autosize');
        }).hide();
    });
}

function showCurrentIdeaImage(ideaId) {
    var $target = $('.ideas-item[data-id="' + ideaId + '"][data-idea-card]');
    $target.find('#idea-text-update').animate({ width: '-=20' }, 0, function () {
        $target.find('#idea-image-file-wraper').show().animate({ width: '+=100' }, 1000, function () {
            $target.find('#idea-text-update').trigger('input.autosize')
        });
        $target.find('#idea-text-update').animate({ width: '-=91' }, 1000);
    });
    $target.find('.highlighter').animate({ width: '-=20' }, 0, function () {
        $('.highlighter').animate({ width: '-=91' }, 1000);
    });
    $target.find('#idea-image-delete').show();
    $target.find('#idea-image-delete').unbind('click');
    $target.find('#idea-image-delete').click(function (e) {

        $target.find('#idea-image-delete').hide();
        //$target.find('.idea-update-image-file').hide();
        $target.find('#idea-image-name').val('');

        $target.find('#idea-text-update').animate({ width: '+=111' }, 1000);
        $target.find('.highlighter').animate({ width: '+=111' }, 1000);
        $target.find('#idea-image-file-wraper').animate({ width: '-=100' }, 1000, function () {
            $target.find('#idea-text-update').trigger('input.autosize');
        }).hide();
    });
}

function changeMediaItem() {
    $('.media-item').removeClass('media-item-selected');
    $(this).addClass('media-item-selected');
    if ($(this).attr('type') == 'video') {
        var yt = $(this).attr('yt');
        $('#map-canvas').hide();
        $('#media-target').show().html('<iframe width="100%" height="100%" src="http://www.youtube.com/embed/' + yt + '?wmode=transparent" frameborder="0" allowfullscreen=""></iframe>');
    }
    else if ($(this).attr('type') == 'map') {
        $('#media-target').hide();
        $('#map-canvas').show();
    }
    else {
        var selected = $(this).find('img').clone();
        selected.attr('src', selected.attr('src').replace('170x105', '683x320'));
        $('#map-canvas').hide();
        $('#media-target').show().html(selected);
    }
}

function systemAlert(title, message, callback) {
    var myAlert = $('#system-alert').clone();
    $(myAlert).find('#alert-title').html(title);
    $(myAlert).find('#alert-message').html(message);
    clearColorbox();
    $.colorbox({
        html: myAlert,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            if (callback) {
                $('.alerta-cerrar').click(callback);
                $('.alerta-aceptar').click(callback);
            }
            else {
                $('.alerta-aceptar').click(closeColorbox);
                $('.alerta-aceptar').click(closeColorbox);
                $('.alerta-cerrar').click(closeColorbox);
                $('.alerta-cerrar').click(closeColorbox);
            }
            resizeColorBox();
        },
        onCleanup: googleClose
    });
}

function systemConfirm(title, message, callback) {
    var myAlert = $('#system-confirm').clone();
    $(myAlert).find('#alert-title').html(title);
    $(myAlert).find('#alert-message').html(message);

    //if ($('#cboxLoadedContent').children().length) {
    //    $boxContent = $('#cboxLoadedContent').children().detach();
    //}

    clearColorbox();
    $.colorbox({
        html: myAlert,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            if (callback) {
                $('.alerta-cerrar').click(function () {
                    //if ($boxContent.length) {
                    //    $('#cboxLoadedContent').html($boxContent);
                    //    resizeColorBox();
                    //    callback(false, false);
                    //}
                    //else {
                    callback(false, true);
                    //}
                });

                $('.alerta-cancelar').click(function () {
                    callback(false, true);
                });

                $('.alerta-aceptar').click(function () {
                    //if ($boxContent.length) {
                    //    $('#cboxLoadedContent').html($boxContent);
                    //    resizeColorBox();
                    //    callback(true, false);
                    //}
                    //else {
                    callback(true, true);
                    //}
                });
            }
            else {
                $('.alerta-cerrar').click(closeColorbox);
                $('.alerta-aceptar').click(closeColorbox);
                $('.alerta-cancelar').click(closeColorbox);
            }
            resizeColorBox();
        },
        onCleanup: googleClose
    });
}

function systemReason(title, message, callback) {
    var $myAlert = $('#system-reason').clone();
    $myAlert.find('#reason-title').html(title);
    $myAlert.find('#reason-message').html(message);
    clearColorbox();
    $.colorbox({
        html: $myAlert,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            if (callback) {
                $myAlert.find('.alerta-cerrar').unbind('click').click(function () { callback('', false); });
                $myAlert.find('.alerta-cancelar').unbind('click').click(function () { callback('', false); });
                $myAlert.find('.alerta-aceptar').unbind('click').click(function () {
                    callback($myAlert.find('#reason-text').val(), true);
                });
            }
            else {
                $myAlert.find('.alerta-cerrar').unbind('click').click(closeColorbox);
                $myAlert.find('.alerta-aceptar').unbind('click').click(closeColorbox);
                $myAlert.find('.alerta-cancelar').unbind('click').click(closeColorbox);
            }

            resizeColorBox();
        },
        onCleanup: googleClose
    });
}

function redirect(url) {
    if (url == '') {
        url = '/';
    }

    document.location.href = url;
}

function popupWindow(url, title, width, height) {
    title = title.replace(/\W*/g, '')
    var left = (screen.width / 2) - (width / 2);
    var top = (screen.height / 2) - (height / 2);
    return (window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left));
}

function validVideoURL(url) {
    var youtubeReg = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*/;
    var vimeoReg = /^.*(vimeo.com\/)([^#\&\?]*).*/;

    var youtubeMatch = url.match(youtubeReg);
    if (youtubeMatch && youtubeMatch[2] && youtubeMatch[2].length == 11) {
        return true;
    }

    var vimeoMatch = url.match(vimeoReg);
    if (vimeoMatch && vimeoMatch[2] && vimeoMatch[2].length > 0) {
        return true;
    }

    return false;
}

function archiveTree() {
    $('.year').unbind('click').click(function (event) {
        $('.month').hide();
        $('.entry').hide();
        $('.month[data-year="' + $(this).attr('data-year') + '"]').show();
    });

    $('.month').unbind('click').click(function (event) {
        $('.entry').hide();
        $('.entry[data-year="' + $(this).attr('data-year') + '"][data-month="' + $(this).attr('data-month') + '"]').show();
    });

    $('.entry').unbind('click').click(function (event) {
        showBlogEntry($(this).attr('data-id'), $(this).attr('data-friendly'));
    });
}

function commentarioBlogEnterKey(event) {
    if (event.keyCode == 13 || event.originalEvent.type == 'click') {

        var input = null;
        if (event.originalEvent.type == 'click') {
            input = $(this).parent().find('textarea');
        }
        else {
            input = $(this);
        }

        if (input.attr('valid-long') != 'true' && input.attr('data-length')) {
            systemAlert('¡Uups!', 'Tu comentario debe tener máximo ' + input.attr('data-length') + ' caracteres.');
            return;
        }

        var id = input.attr('data-id') * 1;
        var size = input.attr('data-size') * 1;
        var container = input.attr('data-container');
        var selector = input.attr('data-selector');
        var tinyScroll = input.attr('data-scroll');
        var paginView = input.attr('data-view');
        var selector = input.attr('data-selector');

        createContentComment(id, input.val(), function (html) {
            input.val('');
            commentsBlogPagin(1, size, id, paginView, tinyScroll, selector, function (html) {
                $(container).html(html);
                renderAdmin();
                if (tinyScroll) {
                    $(tinyScroll).tinyscrollbar_update('relative');
                }
                renderIdeasActions();
                renderAboutUs();
                $('textarea').autosize();
            });
        });

        event.preventDefault();
        return false;
    }
}

function commentsBlogNextPage(pageIndex, pageSize, filterContentId, paginView, tinyScroll, selector, callback) {
    pageIndex = pageIndex + pageSize;
    commentsBlogPagin(pageIndex, pageSize, filterContentId, paginView, tinyScroll, selector, callback);
}

function aboutUsNextPage(pageIndex, pageSize, filterActive, callback) {
    pageIndex = pageIndex + pageSize;
    aboutUsPagin(pageIndex, pageSize, filterActive, callback);
}

function contentBlogNextPage(pageIndex, pageSize, filterContentId, callback) {
    pageIndex = pageIndex + pageSize;
    contentBlogPagin(pageIndex, pageSize, filterContentId, callback);
}

function renderAboutUs() {
    archiveTree();
    validLongComments();
    $('.compartir-a').unbind('click').click(showCompartir);
    $('.blog-entry-click-area').unbind('click').click(function (event) {
        showBlogEntry($(this).attr('data-id'), $(this).attr('data-friendly'));
    });

    if ($('.mm2-create-news').length) {
        $('.mm2-create-news').unbind('click').click(function () {
            newBlogEntry();
        });
    }

    if ($('.edit-blog').length) {
        $('.edit-blog').unbind('click').click(function () {
            var $this = $(this);
            editBlogEntry($this.attr('data-id'));
        });
    }

    if ($('.delete-blog').length) {
        $('.delete-blog').unbind('click').click(function () {
            var $this = $(this);
            systemConfirm($this.data('title-modal'), $this.data('message-modal'), function (result, close) {
                if (result) {
                    deleteBlogEntry($this.attr('data-id'));
                }

                if (close) {
                    closeColorbox();
                }
            });
        });
    }

    if ($('.delete-blog-comment').length) {
        $('.delete-blog-comment').unbind('click').click(function () {
            var $this = $(this);
            systemConfirm($this.data('title-modal'), $this.data('message-modal'), function (result, close) {
                if (result) {
                    deleteBlogCommentEntry($this.attr('data-id'));
                }

                if (close) {
                    closeColorbox();
                }
            });
        });
    }

    if ($('.editar-comentario').length) {
        $('.editar-comentario').unbind('click').click(function (event) {
            var $this = $(this);
            var id = $this.attr('data-id');
            var location = $this.attr('data-location');
            showEditComment(id, location);
        });
    }

    $('.nosotros-contenido-comentario-txt textarea').unbind('keydown').keydown(commentarioBlogEnterKey);
    if ($('.nosotros-contenido').length > 0) {
        clearTimeout(paginCicle);
        paginCicle = setTimeout(function () {
            scrollPagin('.nosotros-contenido', 100, null, function () {
                aboutUsNextPage(1, 6, true);
            })
        }, 100);
    }
}

function vimeoThumbnail(element) {
    var id = element.attr('data-id');
    $.ajax({
        url: 'http://vimeo.com/api/v2/video/' + id + '.json',
        dataType: 'jsonp',
        success: function (data) {
            element.attr('src', data[0].thumbnail_large);
        }
    });
}

function validMail(email) {
    var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    if (email.match(regMail) == null) {
        return false;
    }

    return true;
}

function showPrivacy() {
    if ($(window).width() > 768) {
        var myPrivacy = $('#system-privacy').clone();
        clearColorbox();
        $.colorbox({
            html: myPrivacy,
            fixed: true,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            iframe: false,
            close: null,
            onComplete: function () {
                $('.alerta-cerrar').click(closeColorbox);
                $('.alerta-aceptar').click(closeColorbox);
                $(myPrivacy).find('#scrollbar1').tinyscrollbar();
                resizeColorBox();
            }
        });
    }
    else {
        openTab(path + '/legal/privacy');
    }
}

function renderIdeasActions() {
    $('.reportar').unbind('click');
    $('.reportar').click(function (event) {
        var id = $(this).attr('data-id');
        showReport(id);
    });
    $('.reportar2').unbind('click');
    $('.reportar2').click(function (event) {
        var id = $(this).attr('data-id');
        showReport(id);
    });

    $('.editar').unbind('click');
    $('.editar').click(function (event) {
        var id = $(this).attr('data-id');
        var type = $(this).attr('data-type');
        showEditIdea(id, type);
    });
    $('.editar2').unbind('click');
    $('.editar2').click(function (event) {
        var id = $(this).attr('data-id');
        var type = $(this).attr('data-type');
        showEditIdea(id, type);
    });

    $('.borrar').unbind('click');
    $('.borrar').click(function (event) {
        var $target = $(this);
        var id = $target.attr('data-id');
        var type = $target.attr('data-type');
        systemConfirm($target.data('title-modal'), $target.data('message-modal'), function (result, close) {
            if (result) {
                deleteIdea(id, function () {
                    $('.ideas-item[data-id="' + id + '"][data-idea-card]').remove();
                    if (masonryInstance) {
                        masonryInstance.masonry();
                    }
                });
            }

            if (close) {
                closeColorbox();
            }
        });
    });
    $('.borrar2').unbind('click');
    $('.borrar2').click(function (event) {
        var $target = $(this);
        var id = $target.attr('data-id');
        var type = $target.attr('data-type');
        systemConfirm($target.data('title-modal'), $target.data('message-modal'), function (result, close) {
            if (result) {
                deleteIdea(id, function () {
                    $('.ideas-item[data-id="' + id + '"][data-idea-card]').remove();
                    if (masonryInstance) {
                        masonryInstance.masonry();
                    }
                });
            }

            if (close) {
                closeColorbox();
            }
        });
    });

    $('.editar-comentario').unbind('click');
    $('.editar-comentario').click(function (event) {
        var $this = $(this);
        var id = $this.attr('data-id');
        var location = $this.attr('data-location');
        showEditComment(id, location);
    });

    $('.borrar-comentario').unbind('click');
    $('.borrar-comentario').click(function (event) {
        var $this = $(this);
        var id = $this.attr('data-id');
        var location = $this.attr('data-location');
        systemConfirm($this.data('title-modal'), $this.data('message-modal'), function (result, close) {
            if (result) {

                var targetClass = 'idea-comentario';

                switch (location) {
                    case 'layer-comments':
                    case 'layer-image-comments':
                        targetClass = 'layer-comentario';
                        break;
                    case 'index-comments':
                        targetClass = 'reto-comentarios';
                        break;
                }

                deleteComment(id, function () {
                    $('.' + targetClass + '[data-id="' + id + '"]').remove();
                    if (masonryInstance) {
                        masonryInstance.masonry();
                    }
                });
            }

            if (close) {
                closeColorbox();
            }
        });
    });

    $('.nomegusta-click').unbind('click');
    $('.nomegusta-click').click(function () {
        ideaVote(this, false);
    });
    $('.megusta-click').unbind('click');
    $('.megusta-click').click(function () {
        ideaVote(this, true);
    });
    $('.compartir-a').unbind('click');
    $('.compartir-a').click(showCompartir);
}

function openTab(url) {
    window.open(url);
}

function random(min, max) {
    result = Math.round(Math.random() * (max - min));
    return result + min;
}

function InFrame() {
    if (window != window.top) {
        return true
    }

    return false;
}

function cropText() {
    setTimeout(function () {
        $.each($('[data-crop-height]'), function (index, obj) {
            if ($(obj).attr('data-crop') != 'true') {
                var $target = $(obj);
                var text = $target.html();
                var lastIndex = 0;
                var height = $target.attr('data-crop-height') * 1;
                while ($target.outerHeight() > height) {
                    lastIndex = text.lastIndexOf(' ');
                    text = text.substring(0, lastIndex);
                    $target.html(text + ' ...');
                }
            }
        });
    }, 10);
}

function showLatestNotifications(event) {
    event.stopPropagation();
    event.preventDefault();

    if (isMobile) {
        redirect(path + '/notificaciones');
    }
    else {
        if ($('.notifications-container').is(':visible')) {
            $('.notifications-container').hide();
        }
        else {
            ajaxController(false);
            $.ajax({
                type: 'POST',
                dataType: 'html',
                url: path + '/perfil/notifications/',
                data: { latest: true, page: 0 }
            }).done(function (html) {
                html = html.trim();
                if (html.length) {
                    var $html = $(html);
                    var $target = $('.notification-list');
                    $target.html($html);
                }

                setTimeout(function () { ajaxController(true); }, 100);
            });

            $('.notifications-container').show().click(function (subEvent) {
                subEvent.stopPropagation();
            });
            $(document).click(function () {
                $(document).unbind('click');
                $('#notifications-container').hide();
                $('#notification-list').empty();
            });
        }
    }
}

function showMenuUserBlock(event) {
    event.stopPropagation();
    event.preventDefault();

    if ($('.menu-user-block').is(':visible')) {
        $('.menu-user-block').hide();
    }
    else {
        $('.menu-user-block').show().click(function (subEvent) {
            subEvent.stopPropagation();
        });
        $(document).click(function () {
            $(document).unbind('click');
            $('.menu-user-block').hide();
        });
    }
}

function notificationsPaging(page) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/perfil/notifications',
        data: {
            latest: false,
            page: page
        }
    }).done(function (html) {
        html = html.trim();
        if (html.length > 0) {
            ga('send', 'pageview', '/paginacion-ideas');
            var $html = $(html);

            if (page == 0) {
                $('#main-notification-list').empty().html($html);
            }
            else {
                $('#main-notification-list').append($html);
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.mm2-item-notifications-big', 200, null, function () {
                    notificationsPaging((page * 1) + 1);
                })
            }, 100);
        }
    });
}

function pickmeupInit() {
    $.pickmeup.format = 'Y-m-d';
    $.pickmeup.locale.daysMin = ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa", "Do"];
    $.pickmeup.locale.months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
    $.pickmeup.locale.monthsShort = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];
}

function checkMasonry() {
    if (masonryInstance) {
        masonryInstance.masonry();
    }

    setTimeout(checkMasonry, 100);
}

function dateDiff(date1, date2) {
    date1 = new Date(date1);
    date2 = new Date(date2);

    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}

// END REGULAR

// YOUTUBE

function onYouTubeIframeAPIReady() {
    playerReady = true;
}

function loadVideo(videoID) {
    if (playerReady) {
        var randomHolder = random(0, 1000);
        $('#player-holder').attr('id', 'player-render-' + randomHolder);
        new YT.Player('player-render-' + randomHolder, {
            height: '100%',
            width: '100%',
            videoId: videoID,
            playerVars: {
                wmode: 'opaque'
            },
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange
            }
        });
    }
    else {
        setTimeout(function () { loadVideo(videoID); }, 10);
    }
}

function loadVideo2(videoID) {
    if (playerReady) {
        var randomHolder = random(0, 1000);
        $('#player-holder2').attr('id', 'player-render2-' + randomHolder);
        new YT.Player('player-render2-' + randomHolder, {
            height: '100%',
            width: '100%',
            videoId: videoID,
            playerVars: {
                wmode: 'opaque'
            },
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange
            }
        });
    }
    else {
        setTimeout(function () { loadVideo2(videoID); }, 10);
    }
}

function onPlayerReady(event) {

}

function onPlayerStateChange(event) {
    if (event.data == YT.PlayerState.PLAYING) {
        $('[data-banner][data-video=true]').hide();
        clearTimeout(bannerTimeout);
    }
}

// END YOUTUBE

// FROMS
function validateUserForm() {
    $('#UserForm').unbind('submit');
    $('#UserForm').submit(function () {
        valid = true;

        //if ($('#Description').attr('valid-log') != 'true') {
        //    valid = false;
        //}

        if ($('#UserForm').valid() && valid == true) {
            return true;
        }
        else {
            if (parent.frames.length > 0) {
                resizeColorBox($(document).width(), $(document).height())
            }
            else {
                resizeColorBox();
            }

            return false;
        }
    });

    $('#UserForm').submit();
}

function validateContactForm() {
    $('#ConactUsForm').unbind('submit');
    $('#ConactUsForm').submit(function () {
        if ($('#ConactUsForm').valid()) {
            return true;
        }
        else {
            if (parent.frames.length > 0) {
                resizeColorBox($(document).width(), $(document).height())
            }
            else {
                resizeColorBox();
            }

            return false;
        }
    });

    $('#ConactUsForm').submit();
}

function sendManualEmail() {
    var userId = $('#email-user-id').val();
    var contentId = $('#email-content-id').val();
    var name = $('#email-name').val();
    var subject = $('#email-subject').val();
    var body = $('#email-html-editor').val();
    var valid = true;

    if (subject.trim().length) {
        $('#error-subject').hide();
    }
    else {
        $('#error-subject').show();
        valid = false;
    }

    if (body.trim().length) {
        $('#error-body').hide();
    }
    else {
        $('#error-body').show();
        valid = false;
    }

    if (valid) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/mail/manualemail',
            data: {
                userId: userId,
                contentId: contentId,
                name: name,
                subject: subject,
                body: body
            }
        }).done(function (json) {
            if (json.result) {
                parent.ga('send', 'pageview', '/envio-correo/');
                parent.systemAlert('¡Correo enviado!', 'Se ha enviado el correo.');
            }
            else {
                systemAlert('¡Uups!', 'Esta acción no se pudo realizar.');
            }
        });
    }
}
// END // FROMS

// COLORBOX
function closeColorbox() {
    if (parent.frames.length > 0) {
        if ($boxContent.length) {
            parent.$('#cboxLoadedContent').html($boxContent.shift());
            parent.resizeColorBox();
        }
        else {
            parent.$.colorbox.close();
        }
    }
    else {
        if ($(window).width() > 768) {
            if ($boxContent.length) {
                $('#cboxLoadedContent').html($boxContent.shift());
                resizeColorBox();
            }
            else {
                $.colorbox.close();
            }
        }
        else {
            redirect(path);
        }
    }
}

function resizeColorBox(width, height) {
    if ($.colorbox) {
        var data = { innerWidth: width, innerHeight: height };

        if (width && height) {
            $.colorbox.resize(data);
            parent.$.colorbox.resize(data);
        }
        else {
            var $box = $('#cboxLoadedContent');
            var $conent = $box.children();

            var data = { innerWidth: $conent.width(), innerHeight: $conent.height() };
            $.colorbox.resize(data);
            parent.$.colorbox.resize(data);
        }
    }
}

function clearColorbox() {
    if ($('#cboxLoadedContent').children().length) {
        $boxContent[$boxContent.length] = $('#cboxLoadedContent').children().detach();
    }
    //$('#cboxLoadedContent').empty();
}
// END COLORBOX

// JSON CALLS
function cancelNotification(token) {
    systemConfirm('¿Cancelar suscripción?', '¿estás seguro de cancelar la suscripción a este correo?', function (result, close) {
        if (result) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: path + '/perfil/cancelnotificacion',
                data: {
                    id: token
                }
            }).done(function (json) {
                if (json.result) {
                    ga('send', 'pageview', '/cancelar-notificacion/');
                    systemAlert('¡Cancelación exitosa!', 'Ya no recibirás mas este correo.', function () {
                        $boxContent = [];
                        closeColorbox();
                    });
                }
                else {
                    systemAlert('¡Uups!', 'Esta acción no se pudo realizar.', function () {
                        $boxContent = [];
                        closeColorbox();
                    });
                }
            });
        }
    });
}

function cleanUser($target) {
    var admin = $target.attr('data-admin');
    var messages1 = $target.attr('data-mensaje1');
    var messages2 = $target.attr('data-mensaje2');
    var messages3 = $target.attr('data-mensaje3');
    var messagesencabezado1 = $target.attr('data-mensajeencabezado');
    var messagesencabezado2 = $target.attr('data-mensajeencabezado2');
    var firstConfirm = admin == 'true' ? messagesencabezado1 : messagesencabezado1;
    var secondConfirm = admin == 'true' ? messagesencabezado2 : messagesencabezado2;
    var firstDescription = admin == 'true' ? messages1 : messages1;
    var secondDescription = admin == 'true' ? messages2 : messages3;

    systemConfirm(firstConfirm, firstDescription, function (result, close) {
        if (result) {
            systemReason(secondConfirm, secondDescription, function (reason, confirm) {
                if (confirm) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: path + '/perfil/cleanuser',
                        data: {
                            userId: $target.attr('data-id'),
                            reason: reason
                        }
                    }).done(function (json) {
                        if (json.result) {
                            ga('send', 'pageview', '/bajar-usuario/');
                            if (json.close) {
                                redirect(path + '/registro/cerrarsesion');
                            }
                            else {
                                redirect(path + '/');
                            }
                        }
                        else {
                            systemAlert('¡Uups!', 'Esta acción no se pudo realizar.');
                        }
                    });
                }
                else {
                    closeColorbox();
                }
            });
        }
        else {
            closeColorbox();
        }
    });
}

function ideaShare(ideaId, network) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/contenido/compartir',
        data: {
            ideaId: ideaId,
            network: network
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/compartir-idea/' + network);
        }
        else {
        }
    });
}

function answerVote(object) {
    var myObject = object;
    var answerId = $(object).attr('data-id');
    var messagesvoto = $('#alert-message').attr('data-messagesvoto');
    var messagesvotosave = $('#alert-message').attr('data-messagesvotosave');


    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/contenido/respuesta',
        data: {
            answerId: answerId
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/votar-pregunta/');
            closeColorbox(); hideRegistro();
            redirect(document.location.href);
        } else {
            if (json.error == 403) {
                showEntry(function () { answerVote(myObject); });
            }
            else if (json.error == 102) {
                systemAlert('¡Uups!', messagesvoto, function () { redirect(document.location.href); });
            }
            else {
                systemAlert('¡Uups!', messagesvotosave);
            }
        }
    });
}

function adminAction($element) {
    var $target = $element.parent();
    var id = $target.attr('data-id') * 1;
    var type = $target.attr('data-type');
    var parentType = $target.attr('data-parent-type');
    var action = $element.attr('data-action');
    var email = $target.attr('data-email');
    var userId = $target.attr('data-user-id');
    var location = $target.attr('data-location');
    if (action == 'contact') {
        //redirect('mailto:' + email);
        if (location == 'pulse') {
            showManualEmail(null, id);
        }
        else {
            showManualEmail(userId, null);
        }
        $target.find('.item-div-a').click();
        return;
    }
    if (action == 'contact-all') {
        showManualEmail(null, null);
        $target.find('.item-div-a').click();
        return;
    }
    if (action == 'administration') {
        openTab(path + '/admin');
        $target.find('.item-div-a').click();
        return;
    }
    if (action == 'edit' && location == 'ideas' && type == 'idea') {
        showEditIdea(id, parentType);
        $target.find('.item-div-a').click();
        return;
    }
    if (action == 'edit' && type == 'comment') {
        showEditComment(id, location);
        $target.find('.item-div-a').click();
        return;
    }

    if (action == 'disable' && (type == 'idea' || type == 'comment')) {
        $target.find('.item-div-a').click();
        systemReason('Desactivar ' + (type == 'idea' ? 'idea' : 'comentario'), 'Ingresa una razón', function (reason, result) {
            if (result) {
                sendAdminAction(id, type, action, location, reason, $target);
                closeColorbox();
            }
            else {
                closeColorbox();
            }
        });
    }
    else {
        sendAdminAction(id, type, action, location, '', $target);
    }
}

function sendAdminAction(id, type, action, location, reason, $target) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/contenido/adminaction',
        data: {
            id: id,
            type: type,
            action: action,
            reason: reason
        }
    }).done(function (json) {
        if (json.result) {
            if (action == 'disable') {
                switch (location) {
                    case 'ideas':
                        $target.parent().parent().parent().remove();
                        if (masonryInstance) {
                            masonryInstance.masonry();
                        }
                        break;
                    case 'my-ideas':
                    case 'comments':
                    case 'about-comments':
                    case 'layer-comments':
                    case 'profile-comments':
                    case 'layer-image-comments':
                        $target.parent().parent().parent().parent().remove();
                        if (masonryInstance) {
                            masonryInstance.masonry();
                        }
                        break;
                    case 'top-ideas':
                        $target.parent().parent().parent().parent().parent().parent().remove()
                        break;
                }
            }

            if (type == 'frontend' && action == 'edit' && json.view.length) {
                showEditable(json.view);
            }
        }
        else {
            systemAlert('¡Uups!', 'Esta acción no se pudo realizar.');
        }

        $target.find('.item-div-a').click();
    });
}

function showEditable(view) {
    $.colorbox({
        href: path + '/editable/?view=' + view,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-editable/' + view);
            $('.alerta-cerrar').click(closeColorbox);
            $('.clear-editable-image').unbind('click').click(function () {
                var $this = $(this);
                clearEditableImage($this.attr('data-id'));
            });
            $('.alerta-aceptar').click(function () {
                var valuesJSON = [];
                $.each($('.editable-value'), function (index, obj) {
                    valuesJSON[valuesJSON.length] = {
                        TypeID: $(obj).attr('data-id'),
                        ValueType: $(obj).attr('data-type'),
                        Value: $(obj).val(),
                    };
                });

                var stringJSON = JSON.stringify(valuesJSON);

                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: path + '/editable/savechanges',
                    data: {
                        json: stringJSON
                    }
                }).done(function (json) {
                    if (json.result) {
                        redirect(document.location.href);
                    }
                    else {
                        systemAlert('¡Uups!', 'Esta acción no se pudo realizar.');
                    }
                });

            });
        }
    });
}

function updateUserBlock(callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/home/userblock'
    }).done(function (html) {
        if (html.length > 0) {
            $('#user-block-wraper').html(html);
            $('#user-block-wraper2').html(html);
            if (callback) {
                callback();
            }
        }
    });
}

function joinChallenge(object) {
    var myObject = object;
    var contentId = $(object).attr('data-id');
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/contenido/joinchallenge',
        data: {
            contentId: contentId
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/unirse-reto');
            $(object).removeClass('unirseboton').addClass('unirseboton-usado');
            var followers = $('#challenge-followers').html().split('/');
            var currentFollowers = (followers[0] * 1) + 1;
            $('#challenge-followers').html(currentFollowers + '/' + followers[1]);
        }
        else {
            if (json.error == 403) {
                showEntry(function () { closeColorbox(); hideRegistro(); joinChallenge(myObject); });
            }
            else {
                systemAlert('¡Uups!', 'Aún no estás dentro del reto. ¡Vuelve a intentarlo!');
            }
        }
    });
}

function resetPassword() {
    var password = $('#new-password').val();
    var rePassword = $('#new-re-password').val();
    var token = $('#reset-password').attr('data-token');
    if (password.length == 0) {
        $('#error-new-password').html('Debes ingresar una nueva contraseña');
    }
    else if (password != rePassword) {
        $('#error-new-re-password').html('Las contraseñas no coinciden');
    }
    else {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/registro/reset',
            data: {
                token: token,
                password: password,
                rePassword: rePassword
            }
        }).done(function (json) {
            if (json.result) {
                ga('send', 'pageview', '/recuperar-contrasena-cambio');
                var messagesconfirmpassword = $('#alert-message').attr('data-messagesconfirmarpassword');
                var messagesverygood = $('#alert-message').attr('data-messagesverygood');
                systemAlert(messagesverygood, messagesconfirmpassword, function () {
                    $boxContent = [];
                    closeColorbox();
                });
            } else {
                var messagespasswordsave = $('#alert-message').attr('data-messagespasswordsave');

                systemAlert('¡Uups!', messagespasswordsave, function () {
                    showResetPassword(token);
                });
            }
        });
    }
    resizeColorBox();
}

/// <History>
/// Modificado por      :   Juan Carlos Soto Cruz (JCS)
/// Descripción cambio  :   Se modifica la sobrecarga del systemAlert para que incluya el callback.
/// Fecha               :   2015/11/09
/// </History>
function recoveryPassword() {
    var email = $('#recovery-correo').val();
    var messageserror1 = $('#recovery-error').attr('data-messageserror');
    var messageserror2 = $('#recovery-error').attr('data-messageserror2');
    var sendemail1 = $('#recovery-error').attr('data-sendemail1');
    var sendemail2 = $('#recovery-error').attr('data-sendemail2');
    if (validMail(email)) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/registro/recuperar',
            data: {
                email: email
            }
        }).done(function (json) {
            if (json.result) {
                ga('send', 'pageview', '/recuperar-contrasena-envio');
                systemAlert(sendemail1, sendemail2, function () {
                    $boxContent = [];
                    closeColorbox();
                });
            } else {
                systemAlert('¡Ups!', messageserror2, showRecovery);
            }
        });
    }
    else {
        $('#recovery-error').html(messageserror1);
    }
}

function ideaVote(object, favorable) {
    var myObject = object;
    var myFavorable = favorable;
    var ideaId = $(object).attr('data-id');
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/vote',
        data: {
            ideaId: ideaId,
            favorable: favorable
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/votar-idea');
            $(object).find('.votes').html($(object).find('.votes').html() * 1 + 1);
            $($('.nomegusta-click[data-id="' + ideaId + '"]').parent().find('span.megusta')).css('background-color', '#A7A7A7');
            $($('.nomegusta-click[data-id="' + ideaId + '"]').parent().find('span.nomegusta')).css('background-color', '#A7A7A7');
            $($('.nomegusta-click[data-id="' + ideaId + '"]').parent().find('span.votes')).css('background-color', '#777');
            $('.nomegusta-click[data-id="' + ideaId + '"]').css('background-color', '#777').removeClass('nomegusta-click');
            $('.megusta-click[data-id="' + ideaId + '"] .votes').css('background-color', '#777').removeClass('megusta-click');
            $('.megusta-click[data-id="' + ideaId + '"]').css('background-color', '#A7A7A7').removeClass('megusta-click');
        } else {
            if (json.error == 403) {
                showEntry(function () { closeColorbox(); hideRegistro(); ideaVote(myObject, myFavorable); });
            }
            else if (json.error == 101) {
                systemAlert('¡Uups!', 'Tu voto no ha sido guardado, ¡Los Administradores no pueden votar!');
            }
            else {
                systemAlert('¡Uups!', 'Tu voto no ha sido guardado. ¡Intenta de nuevo!');
            }
        }
    });
}

function updateSocialLink(object) {
    var data;
    switch ($(object).attr('data-id')) {
        case 'facebook':
            data = { facebook: $(object).find('input').val() }
            break;
        case 'google':
            data = { google: $(object).find('input').val() }
            break;
        case 'twitter':
            data = { twitter: $(object).find('input').val() }
            break;
        case 'linkedin':
            data = { linkedin: $(object).find('input').val() }
            break;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/registro/updatesociallink',
        data: data
    }).done(function (json) {
        if (json.result) {
            $(object).find('h2').show();
            $(object).find('input').hide();
            $(object).find('.blog-enlaces-plus').hide();
            $(object).attr('data-text', $(object).find('input').val());
            $(object).find('input').val('');
        } else {
            var messagesredessociales = $('#alert-message').attr('data-messagesredessociales');
            systemAlert('¡Uups!', messagesredessociales);
            $(object).find('h2').show();
            $(object).find('input').hide().val('');
            $(object).find('.blog-enlaces-plus').hide();
        }
    });
}

function entryUser(container, callback) {
    var email = $(container + ' #entry-email').val();
    var password = $(container + ' #entry-password').val();
    var phone = $(container + ' #entry-phone').val();
    var valid = true;

    if ($(container + ' #entry-phone').is(':visible')) {
        if (phone.length < 10) {
            //$(container + ' #entry-phone-error').html('Ingresa un número celular válido');
            $(container + ' #entry-phone-error').css("display", "block");
            valid = false;
        }
        else {
            $(container + ' #entry-phone-error').css("display", "none");
        }
    }
    else {
        if (!validMail(email)) {
            $(container + ' #entry-mail-error').css("display", "block");
            valid = false;
        }
        else {
            $(container + ' #entry-mail-error').css("display", "none");

        }
        if (password.length < 1) {
            $(container + ' #entry-password-error').css("display", "block");
            valid = false;
        }
        else {
            $(container + ' #entry-password-error').css("display", "none");
        }

    }

    if (!valid) {
        resizeColorBox();
        return false;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/registro/ingreso',
        data: {
            email: email,
            password: password,
            phone: phone
        }
    }).done(function (json) {
        var messagespasswordinvalido = $('#alert-message').attr('data-messagespassworderror');
        var messagestitleerror = $('#alert-message').attr('data-messagestitleerror');
        var messagesmovilinvalido = $('#alert-message').attr('data-messagesmovilinvalido');
        var messagesblock = $('#alert-message').attr('data-messagesblock');
        if (json.result) {



            if (json.authenticated == 1) {
                ga('send', 'pageview', '/usuario-autentica-formulario');
                redirect(document.location.href);
            }
            else if (json.authenticated == 2) {
                systemAlert('¡Uups!', messagesblock);
            }
            else if (json.authenticated == 3) {
                systemAlert('¡Uups!', messagesmovilinvalido);
            }
            else if (json.authenticated == 4) {
                createCookie('phone-registry', phone, 0);
                showRegistry(callback);
            }
        } else {
            systemAlert(messagestitleerror, messagespasswordinvalido, function () { showEntry(callback); });
        }
    });
}

function createIdea(callback) {
    var text = $('#idea-text').val();
    var messagesmin20 = $('#alert-message').attr('data-messagesmin20');
    var messagesmaxidea = $('#alert-message').attr('data-messagesmaxidea');
    var messagescaracteres = $('#alert-message').attr('data-messagescaracteres');
    var messagesselectmap = $('#alert-message').attr('data-messagesselectmap');
    var messagesvideoinvalido = $('#alert-message').attr('data-messagesvideoinvalido');
    if (text.length < 20) {
        systemAlert('¡Uups!', messagesmin20);
        return;
    }

    if ($('#idea-text').attr('valid-long') != 'true') {
        systemAlert('¡Uups!', messagesmaxidea + $('#idea-text').attr('data-length') + messagescaracteres);
        return;
    }

    if ($('#map-canvas').length > 0 && $('#Answer_YCoordinate').val().length == 0) {
        systemAlert('¡Uups!', messagesselectmap);
        return;
    }

    var contentId = $('#idea-content-id').val();
    var image = $('#idea-image-name').val();
    var video = $('#idea-video-url').val();
    var yCoordinate = $('#Answer_YCoordinate').val();
    var xCoordinate = $('#Answer_XCoordinate').val();
    if (video && !validVideoURL(video)) {
        systemAlert('¡Uups!', messagesvideoinvalido);
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/crear',
        data: {
            text: text,
            contentId: contentId,
            image: image,
            video: video,
            xCoordinate: xCoordinate,
            yCoordinate: yCoordinate
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/crear-idea');
            if (callback) {
                callback();
            }
            else {
                redirect(document.location.href);
            }
        }
        else {
            if (json.error == 403) {
                showEntry(function () { closeColorbox(); hideRegistro(); createIdea(callback) });
            }
            else {
                systemAlert('¡Uups!', 'Tu idea aún no ha ingresado. ¡Vuelve a intentarlo!');
            }
        }
    });
}

function updateIdea($target, callback) {
    var $textarea = $target.find('#idea-text-update');

    var messagesmin20 = $('#alert-message').attr('data-messagesmin20');
    var messagesmaxidea = $('#alert-message').attr('data-messagesmaxidea');
    var messagescaracteres = $('#alert-message').attr('data-messagescaracteres');

    var text = $textarea.val();
    if (text.length < 20) {
        systemAlert('¡Uups!', messagesmin20);
        return;
    }

    if ($textarea.attr('valid-long') != 'true') {
        systemAlert('¡Uups!', messagesmaxidea + $textarea.attr('data-length') + messagescaracteres);
        return;
    }

    //if ($('#map-canvas').length > 0 && $('#Answer_YCoordinate').val().length == 0) {
    //    systemAlert('¡Uups!', 'Por favor selecciona una ubicación en el mapa.');
    //    return;
    //}

    var ideaId = $target.find('#edit-idea-id').val();
    var image = $target.find('#idea-image-name').val();
    var video = $target.find('#idea-video-url').val();

    //var yCoordinate = $('#Answer_YCoordinate').val();
    //var xCoordinate = $('#Answer_XCoordinate').val();

    if (video && !validVideoURL(video)) {
        systemAlert('¡Uups!', 'Ingresa un vídeo válido.');
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/editar',
        data: {
            text: text,
            ideaId: ideaId,
            image: image,
            video: video//,
            //xCoordinate: xCoordinate,
            //yCoordinate: yCoordinate
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/editar-idea');
            if (callback) {
                callback();
            }
            else {
                redirect(document.location.href);
            }
        }
        else {
            systemAlert('¡Uups!', 'Tu idea no se actualizo. ¡Vuelve a intentarlo!');
        }
    });
}

function deleteIdea(ideaId, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/borrar',
        data: {
            ideaId: ideaId
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/borrar-idea');
            if (callback) {
                callback();
            }
        }
        else {
            systemAlert('¡Uups!', 'Tu idea no se borro. ¡Vuelve a intentarlo!');
        }
    });
}

function createComment(ideaId, text, callback) {
    if (text != '') {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/comment/crear',
            data: {
                ideaId: ideaId,
                text: text
            }
        }).done(function (json) {
            if (json.result) {
                ga('send', 'pageview', '/crear-comentario-idea');
                if (callback) {
                    callback();
                }
                else {
                    redirect(document.location.href);
                }
            } else {
                if (json.error == 403) {
                    showEntry(function () { closeColorbox(); hideRegistro(); createComment(ideaId, text, callback); });
                }
                else {
                    systemAlert('......', 'Tu comentario aún no ha ingresado. ¡Vuelve a intentarlo!');
                }
            }
        });
    }
    else {
        systemAlert('......', 'Debes ingresar un comentario.');
    }
}

function updateComment(commentId, text, callback) {
    if (text != '') {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/comment/editar',
            data: {
                commentId: commentId,
                text: text
            }
        }).done(function (json) {
            if (json.result) {
                ga('send', 'pageview', '/editar-comentario-idea');
                if (callback) {
                    callback();
                }
                else {
                    redirect(document.location.href);
                }
            }
            else {
                systemAlert('......', 'Tu comentario aún no se ha actualizado. ¡Vuelve a intentarlo!');
            }
        });
    }
    else {
        systemAlert('......', 'Debes ingresar un comentario.');
    }
}

function deleteComment(commentId, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/comment/borrar',
        data: {
            commentId: commentId
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/borrar-comentario');
            if (callback) {
                callback();
            }
        }
        else {
            systemAlert('¡Uups!', 'Tu comentario no se borro. ¡Vuelve a intentarlo!');
        }
    });
}

function voteVersus(voteIdeaId) {
    var contentId = $('#versus-contentId').val();
    var ideaIdA = $('#ideaIdA').val();
    var ideaIdB = $('#ideaIdB').val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/votarversus',
        data: {
            contentId: contentId,
            ideaIdA: ideaIdA,
            ideaIdB: ideaIdB,
            voteIdeaId: voteIdeaId
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/votar-versus');
            newVersus(contentId);
            //closeColorbox();
        } else {
            if (json.error == 403) {
                showEntry(function () { closeColorbox(); hideRegistro(); voteVersus(voteIdeaId); });
            }
            else {
                systemAlert('¡Uups!', 'Aún no registramos tu voto. ¡Intenta de nuevo!');
            }
        }
    });
}

function createContentComment(contentId, text, callback) {
    if (text != '') {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/comment/crearcontent',
            data: {
                contentId: contentId,
                text: text
            }
        }).done(function (json) {
            if (json.result) {
                ga('send', 'pageview', '/crear-comentario-blog/');
                if (callback) {
                    callback();
                }
                else {
                    redirect(document.location.href);
                }
            } else {
                if (json.error == 403) {
                    showEntry(function () { closeColorbox(); hideRegistro(); createContentComment(contentId, text, callback); });
                }
                else {
                    systemAlert('......', 'Aún no registramos tu comentario. ¡Intenta de nuevo!');
                }
            }
        });
    }
    else {
        systemAlert('......', 'Debes ingresar un comentario.');
    }
}

function reportIdea(ideaId, text, motive, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/reportaridea',
        data: {
            ideaId: ideaId,
            text: text,
            motive: motive
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/reportar-idea/' + ideaId);
            if (callback) {
                callback();
            }
            else {
                redirect(document.location.href);
            }
        } else {
            if (json.error == 403) {
                showEntry(function () { closeColorbox(); hideRegistro(); reportIdea(ideaId, text, motive, callback); });
            }
            else {
                systemAlert('......', 'Aún no insertamos tu reporte ¡Vuelve a intentarlo!');
            }
        }
    });
}

function checkVersus(contentId, callback) {
    //var contentId = $('.idea-versus').attr('data-id');;

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/idea/checkversus',
        data: {
            contentId: contentId
        }
    }).done(function (json) {
        if (callback) {
            callback(json);
            return;
        }

        if (!json.result) {
            $('.vrs-block').hide();
        }
    });
}

// VIEW CALLS

function loadBlogEntry(id) {
    var $target = $('.nosotros-contenido[data-id="' + id + '"]');
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/seccion/nosotrosbyid',
        data: {
            id: id
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/update-nosotros');
            var $html = $(html);
            if ($target.length) {
                $target.replaceWith($html);
            }
            else {
                $('#new-blog-content').prepend($html);
            }
            renderAboutUs();
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#nosotros-blog-list-container').empty();
            }
        }
    });
}

function deleteBlogEntry(id) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/blogentry/delete',
        data: {
            id: id
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/delete-nosotros');
            $('.nosotros-contenido[data-id="' + id + '"]').remove();
            closeColorbox();
        }
    });
}

function deleteBlogCommentEntry(id) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/blogentry/deletecomment',
        data: {
            id: id
        }
    }).done(function (json) {
        if (json.result) {
            ga('send', 'pageview', '/delete-comentario-nosotros');
            $('.nosotros-contenido-comentario[data-id="' + id + '"]').remove();
            closeColorbox();
        }
    });
}

function editBlogEntry(id) {
    clearColorbox();
    $.colorbox({
        href: path + '/blogentry/detail/' + id,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: true,
        width: 640,
        height: 573,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-crear-noticia');
            $('.alerta-cerrar').click(closeColorbox);
            resizeColorBox();
            path + '/blogentry/detail/' + id
        }
    });
}

function newBlogEntry() {
    clearColorbox();
    $.colorbox({
        href: path + '/blogentry',
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: true,
        width: 640,
        height: 573,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-crear-noticia');
            $('.alerta-cerrar').click(closeColorbox);
            resizeColorBox();
        }
    });
}

function showManualEmail(userId, contentId) {
    clearColorbox();
    $.colorbox({
        href: path + '/mail/manualemail?userId=' + userId + '&contentId=' + contentId,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: true,
        width: 640,
        height: 593,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-correo-manual');
            $('.alerta-cerrar').click(closeColorbox);

            resizeColorBox();
        }
    });
}

function showTerms() {
    if ($(window).width() > 768) {
        var myTerms = $('#system-terms').clone();
        clearColorbox();
        $.colorbox({
            html: myTerms,
            fixed: true,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            iframe: false,
            close: null,
            onComplete: function () {
                ga('send', 'pageview', '/terminos');
                $('.alerta-cerrar').click(closeColorbox);
                $('.alerta-aceptar').click(closeColorbox);
                $(myTerms).find('#scrollbar1').tinyscrollbar();
                resizeColorBox();
            }
        });
    }
    else {
        openTab(path + '/legal/terms');
    }
}

function showContactUs() {
    if ($(window).width() > 768) {
        clearColorbox();
        $.colorbox({
            href: path + '/home/contacto',
            fixed: false,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            iframe: true,
            width: 801,
            height: 573,
            close: null,
            onComplete: function () {
                ga('send', 'pageview', '/contacto');
                resizeColorBox();
            }
        });
    }
    else {
        redirect(path + '/home/contacto');
    }
}

function showResetPassword(token) {
    clearColorbox();
    $.colorbox({
        href: path + '/registro/reset?token=' + token,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-recuperar-contrasena-cambio');
            $('.alerta-cerrar').click(closeColorbox);
            $('#send-reset-password').click(resetPassword);
            resizeColorBox();
        }
    });
}

function rankingPaging(page) {
    $('#statistics-date').pickmeup();
    var start = null;
    var end = null;

    if ($('#statistics-date').val().length) {
        start = $('#statistics-date').pickmeup('get_date', true)[0];
        end = $('#statistics-date').pickmeup('get_date', true)[1];
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/statistics/ranking',
        data: {
            page: page,
            start: start,
            end: end
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-ranking-estadisticas');
            var $html = $(html);
            var $target = $('#statistics-ranking');

            if (page == 0) {
                $target.empty().html($html);
            }
            else {
                $target.append($html);
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.mm2-item-list-ranking-stadistics', 200, null, function () {
                    rankingPaging((page * 1) + 1);
                })
            }, 100);
        }
    });
}

function categoryPaging(page) {
    $('#statistics-date').pickmeup();
    var start = null;
    var end = null;

    if ($('#statistics-date').val().length) {
        start = $('#statistics-date').pickmeup('get_date', true)[0];
        end = $('#statistics-date').pickmeup('get_date', true)[1];
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/statistics/category',
        data: {
            page: page,
            start: start,
            end: end
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-categorias-estadisticas');
            var $html = $(html);
            var $target = $('#statistics-category');

            if (page == 0) {
                $target.empty().html($html);
            }
            else {
                $target.append($html);
            }
            animatebar('category');
            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.mm2-container-barraest', 200, null, function () {
                    categoryPaging((page * 1) + 1);
                })
            }, 100);
        }
    });
}

function professionPaging(page) {
    $('#statistics-date').pickmeup();
    var start = null;
    var end = null;

    if ($('#statistics-date').val().length) {
        start = $('#statistics-date').pickmeup('get_date', true)[0];
        end = $('#statistics-date').pickmeup('get_date', true)[1];
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/statistics/profession',
        data: {
            page: page,
            start: start,
            end: end
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-profesiones-estadisticas');
            var $html = $(html);
            var $target = $('#statistics-profession');

            if (page == 0) {
                $target.empty().html($html);
            }
            else {
                $target.append($html);
            }
            animatebar('profession');
            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.mm2-container-barraest', 200, null, function () {
                    professionPaging((page * 1) + 1);
                })
            }, 100);
        }
    });
}

function hashtagPaging(page) {
    $('#statistics-date').pickmeup();
    var start = null;
    var end = null;

    if ($('#statistics-date').val().length) {
        start = $('#statistics-date').pickmeup('get_date', true)[0];
        end = $('#statistics-date').pickmeup('get_date', true)[1];
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/statistics/hashtag',
        data: {
            page: page,
            start: start,
            end: end
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-tendencias-estadisticas');
            var $html = $(html);
            var $target = $('#statistics-hashtag');

            if (page == 0) {
                $target.empty().html($html);
            }
            else {
                $target.append($html);
            }
            animatebar('hashtag');
            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.mm2-container-barraest', 200, null, function () {
                    hashtagPaging((page * 1) + 1);
                })
            }, 100);
        }
    });
}

function userPaging(page, contentId, text, filter) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/perfil/users',
        data: {
            page: page,
            contentId: contentId,
            text: text,
            filter: filter
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-usuarios');
            var $html = $(html);

            if (page == 0) {
                if ($('#idea-block')) {

                }

                $('#search-result').empty().html($html).find('#search-list').masonry({ "gutter": 11 });
                masonryInstance = $('#search-result').find('#search-list');
            }
            else {
                masonryInstance.append($html).masonry('appended', $html).masonry();
                //masonryInstance = $('#search-list');
            }

            renderAdmin();
            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.user-card', 600, null, function () {
                    var filter2 = null;
                    if ($('#send-search').attr('data-small') == 'true') {
                        filter2 = 1;
                    }
                    else {
                        filter2 = filter;
                    }

                    userPaging((page * 1) + 1, contentId, text, filter2);
                })
            }, 100);
        }
    });
}

function ideasPaging(page, contentId, text) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/idea/ideas',
        data: {
            page: page,
            contentId: contentId,
            text: text
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-ideas');
            var $html = $(html);

            if (page == 0) {
                $('#search-result').empty().html($html).find('#search-list').masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                //$('#search-list').masonry({ columnWidth: ideaColumnWidth });
                masonryInstance = $('#search-result').find('#search-list');
            }
            else {
                masonryInstance.append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                //masonryInstance = $('#search-list');
            }

            renderAdmin();
            renderIdeas();

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.ideas-item', 200, null, function () {
                    ideasPaging((page * 1) + 1, contentId, text);
                })
            }, 100);
        }
    });
}

function pulsesPagin(page, text) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/contenido/pulses',
        data: {
            page: page,
            text: text
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-pulsos');
            var $html = $(html);
            var continuePaging = true;

            var $button = $('<div class="show-all-wraper"><div id="show-all-pulses" class="show-all-pulses">Ver más</div></div>');

            $button.click(function () {
                $('.contenido-historia').show();

                $button.remove();
                if (page == 0) {
                    $html.find('#search-list .contenido-historia').show();
                }
                else {
                    $html.show();
                }

                clearTimeout(paginCicle);
                paginCicle = setTimeout(function () {
                    scrollPagin('.contenido-historia', 200, null, function () {
                        pulsesPagin((page * 1) + 1, text);
                    })
                }, 100);
            });

            if (page == 0 && stopPulses) {
                $('#search-result').empty().html($html);
                renderAdmin();
                renderTooptip();
                if ($html.find('#search-list .contenido-historia .finalizado').length >= 3) {
                    $('#search-list').append($button);
                    $html.find('#search-list .contenido-historia:nth-child(4)').hide();
                    $html.find('#search-list .contenido-historia:nth-child(5)').hide();
                    $html.find('#search-list .contenido-historia:nth-child(6)').hide();
                    clearTimeout(paginCicle);
                    continuePaging = false;
                    stopPulses = false;
                }
                else if ($html.find('#search-list .contenido-historia .finalizado').length) {
                    $('#search-list').append($button);
                    clearTimeout(paginCicle);
                    continuePaging = false;
                    stopPulses = false;
                }
            }
            else if (stopPulses) {
                $('#search-list').append($html);
                renderTooptip();
                if ($html.find('.contenido-historia .finalizado').length == 6) {
                    $('#search-list').append($button);
                    $html.find('.contenido-historia').hide();
                    clearTimeout(paginCicle);
                    continuePaging = false;
                    stopPulses = false;
                }
                else if ($html.find('.contenido-historia .finalizado').length >= 3) {
                    $('#search-list').append($button);
                    $html.find('.contenido-historia:nth-child(4)').hide();
                    $html.find('.contenido-historia:nth-child(5)').hide();
                    $html.find('.contenido-historia:nth-child(6)').hide();
                    clearTimeout(paginCicle);
                    continuePaging = false;
                    stopPulses = false;
                }
            }
            else if (page == 0) {
                $('#search-result').empty().html($html);
                renderAdmin();
                renderTooptip();
            }
            else {
                $('#search-list').append($html);
                renderTooptip();
            }

            animatebar();

            if (continuePaging) {
                clearTimeout(paginCicle);
                paginCicle = setTimeout(function () {
                    scrollPagin('.contenido-historia', 200, null, function () {
                        pulsesPagin((page * 1) + 1, text);
                    })
                }, 100);
            }
        }
    });
}

function myIdeasPagin(pageIndex, pageSize, userId, filter, url, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + url,
        data: {
            userId: userId,
            pageIndex: pageIndex,
            pageSize: pageSize,
            filter: filter
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-mis-ideas');
            var $html = $(html);
            if (callback) {
                callback($html);
            }
            else {
                $('#my-ideas-perfil').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#my-ideas-perfil');
                renderMyIdeas();
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.ideas-item', 200, null, function () {
                    myIdeasNextPage(pageIndex, pageSize, userId, filter, url)
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                var alertMessage = $('#no-conversations-error').val();
                systemAlert('Oh no!!', alertMessage);
                $('#search-list').empty();
            }
        }
    });
}

function commentsPagin(pageIndex, pageSize, ideaId, view, tinyScroll, selector, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/contenido/comentarios',
        data: {
            ideaId: ideaId,
            pageIndex: pageIndex,
            pageSize: pageSize,
            view: view
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-commentarios');
            if (callback) {
                callback(html);
            }
            else {
                $('#comments-list-container').append(html);
                $('#search-list').masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#search-list');
                if (tinyScroll) {
                    $(tinyScroll).tinyscrollbar_update('relative');
                }

                renderCommentsLevel3();
                renderAdmin();
            }

            if (selector == null || selector.length == 0) {
                selector = '.layer-comentario';
            }

            clearTimeout(paginLayerCicle);
            paginLayerCicle = setTimeout(function () {
                scrollPagin(selector, 250, tinyScroll, function () {
                    commentsNextPage(pageIndex, pageSize, ideaId, view, tinyScroll, selector);
                });
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#comments-list-container').empty();
            }
        }
    });
}

function ideasPagin(pageIndex, pageSize, contentId, filter, callback) {
    var view = null;
    if ($('#no-ideas-sorry').length > 0 && callback) {
        view = '_ContentIdeas';
    }

    // random
    var ideasId = new Array();
    if (filter == 1) {
        var arr = $('.ideas-item');
        for (i = 0; i < arr.length; i++) {
            ideasId[i] = $(arr[i]).attr('data-id') * 1;
        }
    }
    else {
        ideasId = [];
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        traditional: true,
        url: path + '/contenido/ideas',
        data: {
            contentId: contentId,
            pageIndex: pageIndex,
            pageSize: pageSize,
            filter: filter,
            view: view,
            ideasId: ideasId
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-ideas');
            var $html = $(html);
            if (callback) {
                callback($html);
            }
            else {
                $('#search-list').append($html).masonry('appended', $html).masonry({ "gutter": 15, columnWidth: ideaColumnWidth });
                masonryInstance = $('#search-list');
                renderIdeas();
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.ideas-item', 100, null, function () {
                    ideasNextPage(pageIndex, pageSize, contentId, filter)
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#search-list').empty();
            }
        }
    });
}

function challengesPagin(pageIndex, pageSize, active, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/seccion/retos',
        data: {
            active: active,
            pageIndex: pageIndex,
            pageSize: pageSize
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-retos');
            if (callback) {
                callback(html);
            }
            else {
                $('#challenges-list-container').append(html);
                animatebar();
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.seccion-reto', 100, null, function () {
                    challengesNextPage(pageIndex, pageSize, active);
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#challenges-list-container').empty();
            }
        }
    });
}

function questionsPagin(pageIndex, pageSize, active, filter, callback) {

    // random
    var questionsId = new Array();
    if (filter == 1) {
        var arr = $('.seccion-pregunta');
        for (i = 0; i < arr.length; i++) {
            questionsId[i] = $(arr[i]).attr('data-id') * 1;
        }
    }

    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/seccion/preguntas',
        traditional: true,
        data: {
            active: active,
            pageIndex: pageIndex,
            pageSize: pageSize,
            filter: filter,
            questionsId: questionsId
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-preguntas');
            if (callback) {
                callback(html);
            }
            else {
                $('#questions-list-container').append(html);
                animatebar();
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.seccion-pregunta', 100, null, function () {
                    questionsNextPage(pageIndex, pageSize, active, filter);
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#questions-list-container').empty();
            }
        }
    });
}

function showIdea(id, friendly) {
    if ($(window).width() > 768 && !isMobile) {
        clearColorbox();
        $.colorbox({
            href: path + '/idea/',
            fixed: true,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            iframe: false,
            close: null,
            data: {

                id: id,
                layer: true
            },
            onComplete: function () {
                ga('send', 'pageview', '/mostrar-idea/' + friendly);
                var scroll = $('#scrollbar2').length > 0 ? '#scrollbar2' : '#scrollbar';
                var view = $('#scrollbar2').length > 0 ? '_LayerIdeaCommentsList' : '_LayerIdeaCommentsListImage';
                $('.layer-cerrar').click(closeColorbox);
                $('#layer-comment-text').keydown(commentarioEnterKey);
                $('.comentar-coment').click(commentarioEnterKey);
                $(scroll).tinyscrollbar();
                imageMargin('.layer-usuario img', 40, 40);
                imageMargin('.layer-comentario img', 32, 32);
                imageMargin('.layer-media img', 560, 515);
                // init comment pagin
                if ($('.layer-comentario img').length > 0) {
                    clearTimeout(paginLayerCicle);
                    paginLayerCicle = setTimeout(function () {
                        scrollPagin('.layer-comentario img', 250, scroll, function () {
                            commentsNextPage(1, 6, $('#filter-idea-id').val(), view, scroll, null);
                        })
                    }, 100);
                }
                renderIdeasActions();
                validLongComments();
                renderAdmin();
                resizeColorBox();
                renderIdeasLinks();
                $('textarea').autosize();
            }
        });
    }
    else {
        redirect(path + '/' + friendly);
    }
}

function showReport(id) {
    clearColorbox();
    $.colorbox({
        href: path + '/idea/reportar/' + id,
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-reportar-idea');
            $('#report-motive').unbind('change').change(function () {
                var $this = $(this);
                if ($this.val() == 'Seleccionar') {
                    $('#report-motive-error').show();
                }
                else {
                    $('#report-motive-error').hide();
                }
            });
            $('.alerta-cerrar').click(closeColorbox);
            $('.reportar-idea').unbind('click');
            $('.reportar-idea').click(function () {
                var ideaId = $('#report-idea-id').attr('data-id');
                var text = $('#report-text').val();
                var motive = $('#report-motive').val();
                if (motive != 'Seleccionar' /*&& text.length >= 20*/) {
                    reportIdea(ideaId, text, motive, function () {
                        systemAlert('¡Muy bien!', 'Tu reporte ha sido enviado al administrador.', function () {
                            $boxContent = [];
                            closeColorbox();
                        });
                    });
                } else {
                    if (motive == 'Seleccionar') {
                        $('#report-motive-error').show();
                    }
                    /*
                    if (text.length < 20) {
                        $('#report-text-error').show();
                    }
                    */
                }
            });
            resizeColorBox();
        }
    });
}

function showEditIdea(id, type) {
    $.ajax({
        type: 'GET',
        dataType: 'html',
        url: path + '/idea/editar',
        cache: false,
        data: { id: id, type: type }
    }).done(function (html) {
        html = html.trim();
        if (html.length) {
            ga('send', 'pageview', '/mostrar-editar-idea');
            var $content = $(html);
            var $target = $('.ideas-item[data-id="' + id + '"][data-idea-card]');

            $content.find('.erase-idea').click(function () {
                $content.remove();
                if (masonryInstance) {
                    masonryInstance.masonry();
                }
            });

            $content.find('#idea-text-update').unbind('input propertychange keyup').bind('input propertychange keyup', function () {
                validLong(this, $content.find('#idea-text-update').attr('data-length') * 1, $content.find('#idea-text-update-counter'), true);
            });

            $content.find('.video').click(function () {
                $content.find('#idea-video-url').show();
                if ($content.find('#idea-image-name').val() != '') {
                    $content.find('#idea-image-delete').click();
                }
            });

            $content.find('#update-idea').click(function (event) {
                updateIdea($target, function () {
                    $.ajax({
                        type: 'POST',
                        dataType: 'html',
                        url: path + '/contenido/idea',
                        data: { ideaId: id }
                    }).done(function (html) {
                        html = html.trim();
                        if (html.length) {
                            var $content = $(html);
                            $target.replaceWith($content);
                            var $image = $content.find('div[data-friendly] img');
                            if ($image.length) {
                                $image.load(function () {
                                    masonryInstance.masonry('reloadItems');
                                    masonryInstance.masonry();
                                });
                            }
                            else {
                                masonryInstance.masonry('reloadItems');
                                masonryInstance.masonry();
                            }

                            renderIdeas();
                        }
                        else {

                        }
                    });
                });
            });

            $target.find('.edit-container').empty().html($content);
            $content.find('#idea-text-update').hashtags();

            if ($content.find('.idea-update-image-file').length && $content.find('.idea-update-image-file').attr('src') && $content.find('.idea-update-image-file').attr('src').length) {
                showCurrentIdeaImage(id);
            }

            if (masonryInstance) {
                masonryInstance.masonry();
            }
        }
        else {
            $('.ideas-item[data-id="' + id + '"][data-idea-card] .edit-container').empty();
        }
    });
}

function showEditComment(id, location) {
    $.ajax({
        type: 'GET',
        dataType: 'html',
        url: path + '/comment/editar/' + id,
        cache: false
    }).done(function (html) {
        html = html.trim();
        if (html.length) {
            ga('send', 'pageview', '/mostrar-editar-commentario');
            var $content = $(html);
            var targetClass = 'idea-comentario';

            switch (location) {
                case 'layer-comments':
                case 'layer-image-comments':
                    targetClass = 'layer-comentario';
                    break;
                case 'index-comments':
                    targetClass = 'reto-comentarios';
                    break;
            }

            var $target = $('.' + targetClass + '[data-id="' + id + '"]');

            $content.find('#comment-text').unbind('keydown').keydown(function (event) {
                var $this = $(this);
                editCommentarioEnterKey(event, location, $this)
            });
            $content.find('.comentar-coment').unbind('click').click(function (event) {
                var $this = $(this);
                editCommentarioEnterKey(event, location, $this)
            });

            $content.find('#comment-text').unbind('input propertychange keyup').bind('input propertychange keyup', function () {
                validLong(this, $content.find('#comment-text').attr('data-length') * 1, $content.find('#comment-idea-counter'), true);
            });

            $target.find('.edit-container-comment').empty().html($content);
            $content.find('#comment-text').trigger('propertychange');
            $('textarea').autosize();
        }
        else {
            $('.ideas-item[data-id="' + id + '"][data-idea-card] .edit-container-comment').empty();
        }

        if (masonryInstance) {
            masonryInstance.masonry();
        }
    });
}

function showEntry(callback) {
    //if ($(window).width() > 768) {
    clearColorbox();
    $.colorbox({
        href: path + '/registro/ingreso',
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-ingreso');
            $('.alerta-cerrar').click(closeColorbox);
            $('.registrarseb').click(function () { showRegistry(callback); });
            $('.olvidolink a').click(showRecovery);
            $('.alerta-aceptar').click(function () {
                entryUser('#layerRegistro', callback);
            });
            $('#layerRegistro #entry-email').keypress(function (event) {
                if (event.keyCode == 13) {
                    entryUser('#layerRegistro', callback);
                }
            });
            $('#layerRegistro #entry-password').keypress(function (event) {
                if (event.keyCode == 13) {
                    entryUser('#layerRegistro', callback);
                }
            });
            $('.googlei').click(function () { googleLogin(callback); });
            $('.twitteri').click(function () { twitterLogin(callback); });
            $('.facebooki').click(function () { facebookLogin(callback); });
            $('.linkedini').click(function () { linkedInLogin(callback); });

            var $layer = $('#layerRegistro');
            if ($layer.length) {
                $layer.find('#show-phone-login').click(function () {
                    $layer.find('#phone-login-fields').toggle();
                    $layer.find('#email-login-fields').toggle();
                    $layer.find('#show-phone-login').toggleClass('phone-icon phone-icon-gray');
                });
            }

            $('[data-only-number]').unbind('keypress').keypress(onlyNumbers);

            resizeColorBox();
        },
        onCleanup: googleClose
    });
    //}
    /*else {
        closeColorbox();
		//showEntry();
        $('.googlei').unbind('click');
        $('.googlei').click(function () { googleLogin(callback); });
        $('.twitteri').unbind('click');
        $('.twitteri').click(function () { twitterLogin(callback); });
        $('.facebooki').unbind('click');
        $('.facebooki').click(function () { facebookLogin(callback); });
        $('.linkedini').unbind('click');
        $('.linkedini').click(function () { linkedInLogin(callback); });
    }*/
}

function showRegistry(callback) {
    globalCallback = callback;
    clearColorbox();
    if ($(window).width() > 768) {
        $.colorbox({
            href: path + '/registro/usuario',
            fixed: false,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            fastIframe: false,
            iframe: true,
            width: 769,
            /*height: 621,*/
            close: null,
            onComplete: function () {
                $('.alerta-cerrar').removeAttr('href');
                $('.alerta-cerrar').click(closeColorbox);

            }
        });
    } else {
        redirect(path + '/registro/usuario');
    }
}

function showRecovery() {
    clearColorbox();
    $.colorbox({
        href: path + '/registro/recuperar',
        fixed: true,
        overlayClose: true,
        escKey: true,
        scrolling: false,
        iframe: false,
        close: null,
        onComplete: function () {
            ga('send', 'pageview', '/mostrar-recuperar-contrasena');
            $('.alerta-cerrar').click(closeColorbox);
            $('#recovery-aceptar').click(recoveryPassword);
            resizeColorBox();
        }
    });
}

function showUpdate(option) {
    if ($(window).width() > 768) {
        clearColorbox();
        $.colorbox({
            href: path + '/registro/actualizar/' + option,
            fixed: false,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            fastIframe: false,
            iframe: true,
            width: 500,
            height: 621,
            close: null,
            onComplete: function () {
                ga('send', 'pageview', '/mostrar-actualizar-perfil');
                $('.alerta-cerrar').click(closeColorbox);
            }
        });
    }
    else {
        redirect(path + '/registro/actualizar/' + option);
    }
}

function showVersus(id, entry) {
    checkVersus(id, function (json) {
        if (json.result) {
            clearColorbox();
            $boxContent = [];
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: path + '/idea/versus/' + id,
                cache: false
            }).done(function (html) {
                $.colorbox({
                    html: html,
                    fixed: true,
                    overlayClose: true,
                    escKey: true,
                    scrolling: false,
                    iframe: false,
                    close: null,
                    onComplete: function () {
                        ga('send', 'pageview', '/mostrar-vs');
                        $('.alerta-cerrar').click(function () {
                            closeColorbox();
                        });
                        $('.ninguna').unbind('click');
                        $('.ninguna').click(function (event) {
                            newVersus(id);
                        });
                        $('.versus-vote-idea').unbind('click');
                        $('.versus-vote-idea').click(function (event) {
                            voteVersus($(this).attr('data-id'));
                        });
                        renderVersusLinks();
                        resizeColorBox();
                    },
                    onClosed: function () {
                        $('#idea-text').focus();
                    }
                });
            });
        }
        else {
            if (json.error == 403 && entry) {
                showEntry(function () { showVersus(id, true); });
            }
        }
    });
}

function newVersus(id) {
    checkVersus(id, function (json) {
        if (json.result) {
            var cacheRadom = random(1, 10000);
            clearColorbox();
            $boxContent = [];
            $.colorbox({
                data: { cache: false },
                href: path + '/idea/versus/' + id + '?r=' + cacheRadom,
                fixed: true,
                overlayClose: true,
                escKey: true,
                scrolling: false,
                iframe: false,
                close: null,
                onComplete: function () {
                    ga('send', 'pageview', '/mostrar-vs');
                    $('.alerta-cerrar').click(function () {
                        closeColorbox();
                    });
                    $('.ninguna').unbind('click');
                    $('.ninguna').click(function (event) {
                        newVersus(id);
                    });
                    $('.versus-vote-idea').unbind('click');
                    $('.versus-vote-idea').click(function (event) {
                        voteVersus($(this).attr('data-id'));
                    });
                    renderVersusLinks();
                    resizeColorBox();
                },
                onClosed: function () {
                    $('#idea-text').focus();
                }
            });
        }
        else {
            $('.vrs-block').hide();
            systemAlert('Oh No!!', 'No tenemos más versus por ahora', function () {
                $boxContent = [];
                closeColorbox();
            });
        }
    });
}

function showBlogEntry(id, friendly) {
    if ($(window).width() > 768 && !isMobile) {
        clearColorbox();
        $.colorbox({
            href: path + '/blog',
            fixed: true,
            overlayClose: true,
            escKey: true,
            scrolling: false,
            iframe: false,
            data: {
                id: id,
                layer: true
            },
            close: null,
            onComplete: function () {
                ga('send', 'pageview', '/mostrar-blog');
                var scroll = $('#scrollbar2').length > 0 ? '#scrollbar2' : '#scrollbar';
                var view = $('#scrollbar2').length > 0 ? '_LayerIdeaCommentsList' : '_LayerIdeaCommentsListImage';
                $('.layer-cerrar').click(closeColorbox);
                $('#layer-comment-text').keydown(commentarioBlogEnterKey);
                $(scroll).tinyscrollbar();
                imageMargin('.layer-usuario img', 40, 40);
                imageMargin('.layer-comentario img', 32, 32);
                imageMargin('.layer-media img', 560, 515);
                $('.compartir-a').unbind('click');
                $('.compartir-a').click(showCompartir);
                // init comment pagin
                if ($('.layer-comentario img').length > 0) {
                    clearTimeout(paginLayerCicle);
                    paginLayerCicle = setTimeout(function () {
                        scrollPagin('.layer-comentario img', 250, scroll, function () {
                            commentsBlogNextPage(1, 6, $('#content-id').val(), view, scroll, null);
                        })
                    }, 100);
                }
                renderAdmin();
                renderIdeasActions();
                resizeColorBox();
                validLongComments();
                $('textarea').autosize();
            }
        });
    }
    else {
        redirect(path + '/' + friendly);
    }
}

function commentsBlogPagin(pageIndex, pageSize, contentId, view, tinyScroll, selector, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/contenido/comentarioscontenido',
        data: {
            contentId: contentId,
            pageIndex: pageIndex,
            pageSize: pageSize,
            view: view
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-comentario-blog');
            if (callback) {
                callback(html);
            }
            else {
                $('#comments-list-container').append(html);
                if (tinyScroll) {
                    $(tinyScroll).tinyscrollbar_update('relative');
                }

                renderIdeasActions();
                renderAdmin();
                renderAboutUs();
                $('textarea').autosize();
            }

            if (selector == null || selector.length == 0) {
                selector = '.layer-comentario';
            }

            clearTimeout(paginLayerCicle);
            paginLayerCicle = setTimeout(function () {
                scrollPagin(selector, 250, tinyScroll, function () {
                    commentsBlogNextPage(pageIndex, pageSize, contentId, view, tinyScroll, selector);
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#comments-list-container').empty();
            }
        }
    });
}

function aboutUsPagin(pageIndex, pageSize, active, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/seccion/nosotros',
        data: {
            active: active,
            pageIndex: pageIndex,
            pageSize: pageSize
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-nosotros');
            if (callback) {
                callback(html);
            }
            else {
                $('#nosotros-blog-list-container').append(html);
                $('textarea').autosize();
                renderAboutUs();
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.nosotros-contenido', 100, null, function () {
                    aboutUsNextPage(pageIndex, pageSize, active);
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#nosotros-blog-list-container').empty();
            }
        }
    });
}

function contentBlogPagin(pageIndex, pageSize, contentId, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: path + '/contenido/entradasblog',
        data: {
            pageIndex: pageIndex,
            pageSize: pageSize,
            contentId: contentId
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            ga('send', 'pageview', '/paginacion-blog');
            if (callback) {
                callback(html);
            }
            else {
                $('#blog-list-container').append(html);
            }

            clearTimeout(paginCicle);
            paginCicle = setTimeout(function () {
                scrollPagin('.blog-item', 100, null, function () {
                    contentBlogNextPage(pageIndex, pageSize, contentId);
                })
            }, 100);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh Oh!!', 'No tenemos ningún resultado con la información que nos pediste.');
                $('#blog-list-container').empty();
            }
        }
    });
}

// COOKIES
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }

    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

// GOOGLE+
function googleLogin(callback) {
    globalCallback = callback;
    var callbackUrl = escape(siteUrlRoot + '/registro/google').replace(/\//g, '%2F');
    var scope = 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email';
    //var clientId = '211665140926.apps.googleusercontent.com'
    var clientId = '389061458705-q8g215c9sk01buijdhiiso21ajj2eijg.apps.googleusercontent.com'
    var request = 'https://accounts.google.com/o/oauth2/auth?scope=' + scope +
        '&response_type=token&redirect_uri=' + callbackUrl +
        '&state&client_id=' + clientId +
        '&hl=es&from_login=1&as=&pli=1&authuser=0';
    googleWindow = popupWindow(request, 'oAuthGoogle', 650, 650);
}

function googleClose() {
    if (googleWindow) {
        if (globalCallback) {
            globalCallback();
        }

        googleWindow.close();
        googleWindow = null;
    }
}

// FACEBOOK
function facebookShare(name, link, picture, caption, description, callback) {
    scroll = $(window).scrollTop();
    clearTimeout(facebookCicle);
    fixPositionFacebookUI();
    FB.ui({
        method: 'feed',
        name: name,
        link: link,
        picture: picture,
        caption: caption,
        description: description
    },
    function (response) {
        if (callback) {
            callback(response);
        }
        //if (response && response.post_id) {
        //    alert('Post was published.');
        //} else {
        //    alert('Post was not published.');
        //}
    });
}

function fixPositionFacebookUI() {
    if ($('.fb_dialog_advanced').length > 0 && scroll > $(window).scrollTop()) {
        $(window).scrollTop(scroll);
    }
    else {
        facebookCicle = setTimeout(function () { fixPositionFacebookUI() }, 1);
    }
}

function getAccessToken(callback) {
    FB.getLoginStatus(function (response) {
        if (response.authResponse) {
            callback(response.authResponse.accessToken);
            return;
        } else {
            callback(null);
            return;
        }
    });
};

function facebookLogin(callback) {
    getAccessToken(function (token) {
        if (token != null) {
            facebookAuthenticateUser(token, callback);
        }
        else {
            FB.login(function (response) {
                if (response.authResponse) {
                    facebookAuthenticateUser(response.authResponse.accessToken, callback);
                }
            },
            {
                scope: 'email'
            });
        }
    });
}

function facebookAuthenticateUser(token, callback) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: path + '/registro/facebook',
        data: {
            token: token
        }
    }).done(function (json) {
        if (json.result) {
            if (json.authenticated == 1) {
                if (callback) {
                    callback();
                }
                else {
                    redirect(document.location.href);
                }
            }
            else if (json.authenticated == 2) {
                systemAlert('!Uups!', 'Estás bloqueado');
            }
            else {
                FB.api('/me', function (response) {
                    createCookie('user-image', 'https://graph.facebook.com/' + response.id + '/picture?width=100&height=100', 0);
                    createCookie('facebook-token', token, 0);

                    FB.api('/me?fields=name,email', function (response) {
                        createCookie('user-name', response.name, 0);
                        createCookie('user-mail', response.email, 0);
                        showRegistry(callback);
                    });
                });
            }
        }
    });
}

// LINKEDIN
function linkedInLogin(callback) {
    IN.User.authorize(function () {
        IN.API.Profile("me").fields(["firstName", "lastName", "pictureUrl"]).result(function (json) {
            linkedInAuthenticateUser(IN.ENV.auth.oauth_token, callback);
        });
    });
}

function linkedInAuthenticateUser(token, callback) {
    var userid;
    IN.API.Profile("me").fields(["id", "firstName", "lastName", "pictureUrl", "publicProfileUrl"]).result(function (result) {
        userid = result.values[0].id;
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: path + '/registro/linkedin',
            data: {
                token: IN.ENV.auth.oauth_token,
                linkedInUserId: userid
            }
        }).done(function (json) {
            if (json.result) {
                if (json.authenticated == 1) {
                    if (callback) {
                        callback();
                    }
                    else {
                        redirect(document.location.href);
                    }
                }
                else if (json.authenticated == 2) {
                    systemAlert('!Uups!', 'Estás bloqueado');
                }
                else {
                    //createCookie('linkedin-token', IN.ENV.auth.oauth_token, 0)
                    //createCookie('user-image', json.values[0].pictureUrl || '//static02.linkedin.com/scds/common/u/img/icon/icon_no_photo_80x80.png', 0);
                    createCookie('linkedin-token', userid, 0)
                    createCookie('user-image', result.values[0].pictureUrl || path + '/files/imagesuser/default.png', 0);
                    createCookie('user-name', result.values[0].firstName + ' ' + result.values[0].lastName, 0);
                    showRegistry(callback);
                }
            }
        });
    })
}

// TWITTER
function twitterLogin() {
    var url = $.ajax({
        type: "POST",
        url: path + '/registro/getlinktwitter/',
        async: false,
        success: function (data) {
            window.open(data.url, 'TwitterAuthWindow', 'status=0,toolbar=0,location=1,menubar=0');
        }
    });
}

// MAPS
function initializeMap() {
    var mapOptions = {
        center: new google.maps.LatLng(6.230833, -75.590556),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
    marker = new google.maps.Marker();
    var userMarker = new google.maps.Marker();
    google.maps.event.addListener(map, 'click', function (event) {
        userMarker.setMap(null);
        userMarker = new google.maps.Marker({
            map: map,
            position: event.latLng
        });
        $('#Answer_XCoordinate').val(event.latLng.lng().toString().replace('.', ','));
        $('#Answer_YCoordinate').val(event.latLng.lat().toString().replace('.', ','));
    });
}

function initializeMapIdea() {
    var mapOptions = {
        center: new google.maps.LatLng(6.230833, -75.590556),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    ideaMap = new google.maps.Map(document.getElementById("map-canvas-idea"), mapOptions);
}

function enableUserMarker() {
    marker = new google.maps.Marker();
    google.maps.event.addListener(map, 'click', function (event) {
        marker.setMap(null);
        marker = new google.maps.Marker({
            map: map,
            position: event.latLng
        });
        $('#Answer_XCoordinate').val(event.latLng.lng().toString().replace('.', ','));
        $('#Answer_YCoordinate').val(event.latLng.lat().toString().replace('.', ','));
    });
}

function setMarker(lng, lat, html) {
    var newMarker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(lat, lng)
    });

    infoWindow = new google.maps.InfoWindow({
        content: html
    });

    google.maps.event.addListener(newMarker, 'click', function () {
        if (infoWindow) {
            infoWindow.close();
        }
        infoWindow.open(map, newMarker);
    });

    google.maps.event.addListener(map, 'click', function () {
        infoWindow.close();
    });
}

function animatebar(id) {
    if (id) {
        var total = 0;
        $('[data-bar-value="' + id + '"]').each(function (index, obj) {
            total += $(obj).attr('data-value') * 1;
        });

        var count = $('[data-bar-percent="' + id + '"]').length;
        $('[data-bar-percent="' + id + '"]').each(function (index, obj) {
            $(obj).attr('data-percent', ($(obj).attr('data-value') * 1) * 100 / total);
        });
    }

    $("[data-percent]").each(function (index, obj) {
        var percent = $(obj).attr('data-percent');
        if (percent >= 0) {
            $(obj).stop().animate({ width: percent + '%' }, 800);
            $(obj).removeAttr('data-percent');
        }
    });
}

function createIdeaIngles(callback) {
    var text = $('#idea-text').val();
    if (text.length < 20) {
        systemAlert('¡Ooops!', 'Your idea must have at least 20 characters.');

        return;
    }
}

function createIdeaEspanol(callback) {
    var text = $('#idea-text').val();
    if (text.length < 20) {

        systemAlert('¡Uups!', 'Tu idea debe tener mínimo 20 caracteres.');
        return;
    }
}
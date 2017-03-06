
$.expr[':'].containsIgnoreCase = function (n, i, m) { return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0; };
$.expr[':'].noContainsIgnoreCase = function (n, i, m) { return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) == -1; };

$(document).ready(function () {
    autoFill();
});

autoFill = function () {
    $('#IContent_Category').keyup(function () {
        var $target = $(this);
        var $container = $('#auto-fill-container');
        if ($target.val().length > 0) {

            $container.find('.auto-fill').hide();
            var $matchs = $container.find('.auto-fill:containsIgnoreCase("' + $target.val() + '")').slice(0, 5).show();

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
    });    

    $('#auto-fill-container .auto-fill').mousedown(function () {        
        var $target = $(this);
        $('#IContent_Category').val($target.html());
        $('#auto-fill-container').hide();
    });

    $('#IContent_Category').blur(function () {        
        var $container = $('#auto-fill-container');
        $container.hide();
    });
}

updatefile = function (obj, isopen) {
    $(obj).hide();
    if (isopen) {
        var inp = $(obj).parent().find('input');
        inp.val($(obj).html());
        inp.show();
        inp.focus();
    } else {
        var spn = $(obj).parent().find('span');
        if ($(obj).val() != '') {
            $.post(pathg + 'Content/UpdateFile/', { fileattachId: $(obj).parent().attr('data-id'), name: $(obj).val() }, function (data) {
                if (data.result) {
                    spn.html($(obj).val());
                }
            });
        }

        spn.show();
    }
};

getlanguages = function () {
    if ($('#ul_languages').css('display') == 'none') {
        $.post(pathg + 'Admin/Home/GetLanguages/', {}, function (data) {
            $('#ul_languages').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ul_languages').append('<li onclick="setLanguage(' + data[i].LanguageId + ')" title="' + data[i].Name + '"><span>' + data[i].Culturename + '</span> - ' + data[i].Name + '</li>');
            }
            $('#ul_languages').show();
        });
    }
    else {
        $('#ul_languages').hide();
    }
};


getlanguages2 = function () {
    if ($('#ul_languages').css('display') == 'none') {
        $.post(pathg +  'Seccion/GetLanguages/', {}, function (data) {
            $('#ul_languages').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ul_languages').append('<li onclick="setLanguage(' + data[i].LanguageId + ')" title="' + data[i].Name + '"><span>' + data[i].Culturename + '</span> - ' + data[i].Name + '</li>');
            }
            $('#ul_languages').show();
        });
    }
    else {
        $('#ul_languages').hide();
    }
};

setLanguage = function (val) {
    $.post(pathg + 'Admin/Home/SetLanguage/', { id: val }, function (data) {
        if (data.result) {
            window.location.href = window.location.href;
        } else {
            alerts("No se pudo cambiar el lenguaje");
        }
    });
};

setLanguage2 = function (val) {
    var actualUrl = window.location.href;
    $.post(pathg +  'Seccion/SetLanguage/', { id: val }, function (data) {
        if (data.result) {
            window.location.href = actualUrl;
        } else {
            alerts("No se pudo cambiar el lenguaje");
        }
    });
};

setfiltrocasosexito = function () {

    var txtCategory = $('#search-text3').val().trim();
    var txtintitucionimplementa = $('#search-text2').val().trim();
    var txtinstitucionfuente = $('#search-text1').val().trim();
    var intidCity = $('#search-type').val();

    $.post('/Seccion/Setfiltrocasosexito/', { idcity : intidCity , category : txtCategory , intitucionimplementa : txtintitucionimplementa , institucionfuente : txtinstitucionfuente }, function (data) {
        if (data.result) {
            window.location.href = window.location.href;
        } else {
            alerts("No existen registros");
        }
    });
};


expand = function (node, ids) {
    if ($(node).attr('src') == pathg + 'resources/images/15min.gif') {
        $(node).attr('src', pathg + 'resources/images/15add.gif');
        var obj = node.parentNode.parentNode.parentNode;
        $(obj.getElementsByTagName('ul')[0]).slideUp('fast', function () { $(this).remove(); });
    }
    else {
        $('#loading').show();
        $.post(pathg + 'Secciones/GetChild/', { id: ids }, function (data) {
            $(node).attr('src', pathg + 'resources/images/15min.gif');
            if (data.Iscontain) {
                var obj = node.parentNode.parentNode.parentNode;
                $(node.parentNode.parentNode.parentNode).append(data.html);
                $(obj.getElementsByTagName('ul')[0]).slideDown('fast', initialize());
            }
            $('#loading').hide();
        });
    }
};

findsubm = function (e, customfunc) {
    var k = null;
    (e.keyCode) ? k = e.keyCode : k = e.which;
    if (k == 13) {
        customfunc();
        e.cancelBubble = true;
        e.returnValue = false;
    }
};

contenido_textarea = "";
valid_long = function (obj, num_caracteres_permitidos, spn) {
    num_caracteres = $(obj).val().length;

    if (num_caracteres <= num_caracteres_permitidos) {
        contenido_textarea = $(obj).val();
        if (spn != null)
            spn.html(num_caracteres_permitidos - num_caracteres);
    } else {
        $(obj).val(contenido_textarea);
    }
};

codeyoutube = function (url) {
    if (url.indexOf('v=') >= 0) {
        var qeu = url.split('v=');
        qeu = qeu[1].split('&');
        if (qeu.length > 0) {
            return qeu[0];
        }
        else return 'youtube.gif';
    }
    else
        return 'youtube.gif';
};

bc_verif = function (event) {
    if ($(this).val().length > 0 && codeyoutube($(this).val()) != 'youtube.gif') {
        idy = codeyoutube($(this).val());
        objImage = new Image();
        objImage.src = 'http://img.youtube.com/vi/' + idy + '/1.jpg';
        var ids = this.field_id;
        objImage.onload = function () {
            $('#' + ids).attr('src', 'http://img.youtube.com/vi/' + idy + '/1.jpg');
            $('#' + ids).attr('width', 35);
            $('#' + ids).attr('height', 30);
        };
    }
};

addEvent = function (obj, type, fn) {
    if (obj.addEventListener) obj.addEventListener(type, fn, false);
    else if (obj.attachEvent) {
        obj["e" + type + fn] = fn;
        obj[type + fn] = function () {
            obj["e" + type + fn](window.event);
        };
        obj.attachEvent("on" + type, obj[type + fn]);
    }
};
bc_newElement = function (tag) {
    return document.createElement(tag);
};
bc_getElement = function (id) {
    return document.getElementById(id);
};
var field_count = 1;
var optionvald = 1; /*1 imagen, 2 buzon*/
bc_init = function (fileId, displayId) {
    try {
        field = bc_getElement(fileId);
        field.display = bc_getElement(displayId);
        if (!field || !field.type || field.type != 'file' || !field.display) return;
        addEvent(field, 'change', bc_addField);
    } catch (ex) {
        bc_handleError(ex);
    }
};
bc_load = function (fileId, displayId, optio) {
    optionvald = optio;
    addEvent(window, 'load', new Function("bc_init('" + fileId + "', '" + displayId + "');"));
};
bc_addFieldvideo = function () {
    li = bc_newElement('LI');

    var pref = $('.txtfilevideo').size();
    inp = bc_newElement('INPUT');
    inp.type = 'TEXT';
    inp.id = 'VID' + pref;
    inp.name = 'videoyoutube';
    inp.field_id = 'VIDIM' + pref;
    inp.className = 'txtfilevideo';
    inp.maxLength = '150';
    addEvent(inp, 'blur', bc_verif);

    img = bc_newElement('IMG');
    img.className = 'del'
    img.src = pathg + 'resources/images/25del.gif';
    img.align = 'right';
    img.width = '20';
    img.height = '20';
    img.title = 'Eliminar video';
    addEvent(img, 'click', bc_removeFieldvideo);

    ico = bc_newElement('IMG');
    ico.id = 'VIDIM' + pref;
    ico.align = 'left';
    ico.src = pathg + 'resources/images/youtube.gif';

    spn = bc_newElement('DIV');
    spn.innerHTML = 'Ingresa la url del video';

    li.appendChild(img);
    li.appendChild(ico);
    li.appendChild(spn);
    li.appendChild(inp);
    bc_getElement('list_files').appendChild(li);
};
bc_addField = function () {
    var obj = $(this.id);
    if (this.id.indexOf('ifEx') == -1) {
        try {
            new_field = bc_newElement('INPUT');
            new_field.type = 'file';
            new_field.id = new_field.name = this.id.replace(/-@bc-.*$/g, "") + '-@bc-' + field_count++;
            new_field.className = 'file';
            new_field.display = this.display;
            addEvent(new_field, 'change', bc_addField);
            this.parentNode.insertBefore(new_field, this);
            li = bc_newElement('LI');

            img = bc_newElement('IMG');
            img.className = 'del'
            img.src = pathg + 'resources/images/25del.gif';
            img.align = 'right';
            img.width = '20';
            img.height = '20';
            img.title = 'Eliminar archivo';
            img.field_id = this.id;
            addEvent(img, 'click', bc_removeField);

            ico = bc_newElement('IMG');
            ico.align = 'left';
            ico.src = pathg + 'resources/images/' + bc_geticon(this.value);

            spn = bc_newElement('DIV');
            spn.innerHTML = ' ' + this.value.substring(this.value.search(/[^\/\\]+$/));

            inp = bc_newElement('INPUT');
            inp.type = 'TEXT';
            inp.name = inp.id = 'NP' + this.name;
            inp.maxLength = '30';
            inp.className = 'txtfiles';

            li.appendChild(img);
            li.appendChild(ico);
            li.appendChild(spn);
            li.appendChild(inp);
            this.display.appendChild(li);
            this.style.display = 'none';
        } catch (ex) {
            bc_handleError(ex);
        }
    } else {
        document.getElementById('txtExaminar').value = this.value;
    }
};
bc_removeField = function (event) {
    try {
        (del = bc_getElement(this.field_id)).parentNode.removeChild(del);
        this.parentNode.parentNode.removeChild(this.parentNode);
        if (event && event.preventDefault) event.preventDefault();
        return false;
    } catch (ex) {
        bc_handleError(ex);
    }
};
bc_removeFieldvideo = function (event) {
    $(this.parentNode).remove();
};
bc_handleError = function (ex) {
    alert(ex);
};
bc_geticon = function (val) {
    var b = val.split('.');
    var ext = b[b.length - 1].toLowerCase();
    switch (ext) {
        case 'jpg': case 'bmp': case 'gif': case 'png':
            return 'image.gif';
        case 'xls': case 'xlsx':
            return 'excel.gif';
        case 'doc': case 'docx':
            return 'word.gif';
        case 'ppt': case 'pptx': case 'pps':
            return 'powerpoint.gif';
        case 'vst':
            return 'visio.gif';
        case 'pdf':
            return 'pdf.gif';
        default:
            return 'default.gif';
    }
};


divlayer = function () {
    document.write('<div onclick="closediv()" id="dvmask"></div>');
    document.write('<div id="dvdialog">');
    document.write('<iframe style="background-image:url(' + pathg + 'resources/images/ajax.gif); background-position:center; background-repeat:no-repeat;" scrolling="no" frameborder="0" id="frm"></iframe>');
    document.write('</div>');
};

opendiv = function (width, heigh, url) {
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();
    var winH = $(window).height();
    var winW = $(window).width();

    $('#dvmask').css({ 'width': maskWidth, 'height': maskHeight });
    $('#dvdialog').css('top', (winH - heigh) / 2);
    $('#dvdialog').css('left', (winW - width) / 2);
    $('#dvdialog').css({ 'width': width, 'height': heigh });
    $('#dvmask').fadeTo(200, 0.5);
    $('#frm').css({ 'width': width, 'height': heigh });
    $('#dvdialog').fadeIn(200);
    $('#frm').show();
    if (url != null)
        $('iframe#frm').attr('src', url);
};

alerts = function (message) {
    $('#dvbodyalert').html(message);
    $('#bckalert').css('height', $(document).height());
    $('#bckalert').show();
    $('#dvalert').show();
};

closealert = function () {
    $('#bckalert').hide();
    $('#dvalert').hide();
};

closediv = function () {
    $('#dvmask').fadeOut(200);
    $('#dvdialog').fadeOut(200, function () {
        $('iframe#frm').attr('src', '');
    });
};

fade = function (value, mod, obj) {
    if (value != null) {
        for (i = 0; i < value.length; i++) {
            var classname = value[i].ModulId == mod ? 'liactive' : '';
            if (!value[i].IsContent) {
                $('#MenuS2').append('<li class="mod' + value[i].ModulId + ' ' + classname + '" onclick="window.location.href=\'' + pathg + 'Admin/' + value[i].Controller + '/Index/\'">' + value[i].Name + '</li>');
            }
            else {
                $('#MenuS2').append('<li class="mod' + value[i].ModulId + ' ' + classname + '" onclick="window.location.href=\'' + pathg + 'Admin/Content/Index/?mod=' + value[i].ModulId + '\'">' + value[i].Name + '</li>');
            }
        }
     
    }

    $('#MenuS2').animate({
        left: '0px'
    }, 700);

    $('#MenuS').animate({
        left: '-190px'
    }, 700);

   
};

back = function () {
    $('#MenuS2').animate({
        left: '190px'
    }, 700, function () { $('#MenuS2').empty(); });

    $('#MenuS').animate({
        left: '0px'
    }, 700);


};

fadesec = function () {
    $('#seccont').animate({
        left: '0px'
    }, 700);

    $('#seccontinfo').animate({
        left: '-500px'
    }, 700);
};

backsec = function () {
    $('#seccont').animate({
        left: '500px'
    }, 700);

    $('#seccontinfo').animate({
        left: '0px'
    }, 700);
};

focusfind = function (obj, val) {
    if ($(obj).val() == val || $(obj).val() == '') {
        $(obj).removeClass('inactive');
        $(obj).val('');
    }
};

blurfind = function (obj, val) {
    if ($(obj).val() == '') {
        $(obj).addClass('inactive');
        $(obj).val(val);
    }
};

dettachcontent = function (childid, contentid, obj) {
    $.post(pathg + 'Content/DettachAjax/', { id: childid, contentId: contentid }, function (data) {
        if (data != null && data.result) {
            $(obj).parent().remove();
        }
    });
};

deletefile = function (Fileid, obj) {
    $.post(pathg + 'Content/Deletefile/', { id: Fileid }, function (data) {
        if (data != null && data.result) {
            $(obj).parent().remove();
        }
    });
};

/* helpers content */
contentclass = function () {
    this.Urlorder = pathg + 'Admin/Content/ChangeOrder';
    this.UrlGetContent = pathg + 'Admin/Content/GetContent';
    this.UrlDeleteContent = pathg + 'Admin/Content/DeleteContent';
    this.UrlChangeSection = pathg + 'Admin/Content/ChangeSection';
    this.UrlNewContent = null;
    this.UrlDetailContent = null;
    this.clicOk = true;
    this.NotFound = null;
    this.TextFind = null;

    this.funcinicializate = function () { };

    this.resizable = function () {
        $("#conttreecontent").resizable({
            maxWidth: 640,
            minWidth: 200
        });
    };

    this.draggable = function () {
        $('#listado span').draggable({
            revert: "invalid",
            autoSize: true,
            ghosting: false
        });
    };

    this.droppable = function () {
        var me = this;
        $('#conttreecontent div').droppable({
            hoverClass: "sectionactive",
            drop: function (event, ui) {
                $('#loading').show();
                var ContentId = ui.draggable.parent().find('input').val();
                var SectionId = $(this).attr('id');
                $.post(me.UrlChangeSection, { contentId: ContentId, sectionId: SectionId }, function (data) {
                    ctnback.binddetail(SectionId);
                });
            }
        });
    }

    this.sortable = function () {
        var url = this.Urlorder;

        $("#listado").sortable({
            opacity: 0.7,
            axis: 'y',
            handle: '.handle',
            stop: function (event, ui) {
                $('#loading').show();
                var source = ui.item.children('.handle');
                var id = source.attr('id');
                var newid = $('#li' + id).prev().children('.handle').attr('id');
                var limit = false;

                if (newid == undefined) {
                    newid = $('#li' + id).next().children('.handle').attr('id');
                    limit = true;
                }

                if (newid != undefined) {
                    $.post(url, { contentId: id, prevId: newid, limit: limit }, function (data) {
                        if (data != null && data.result)
                            $('#loading').hide();
                    });
                }
            }
        });
        $("#listado").disableSelection();
    };

    this.contentpagin = function (mod) {
        var findname = ($('#txname').val() != '' && $('#txname').val() != this.TextFind) ? $('#txname').val() : undefined;
        var pag = parseInt($('#idPage').val());
        var total = parseInt($('#idCount').val());
        var result = parseInt($('#spnresult').html());

        var me = this;

        if (total != result) {
            $('#loading').show();
            pag++;
            $.post(this.UrlGetContent, { mod: mod, page: pag, sectionId: $('#SectionId').val(), active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), name: findname }, function (data) {
                $('#listado').append(data.html);
                $('#idCount').val(total + data.count);
                $('#spnnumregis').html($('#idCount').val());
                $('#loading').hide();
                $('#idPage').val(pag);
                me.draggable();
                me.resizesection();
                if (data.count == 0) {
                    $('#listado').append('<li>' + me.NotFound + '</li>');
                }
            });
        }
    };

    this.binddetail = function (val) {
        $('#SectionId').val(val != 0 ? val : null);
        $('#conttreecontent').find('.cll').remove();
        $('#conttreecontent').find('.activ').removeClass('activ');
        $('#' + val + ' > nobr > img').after('<img class="cll" src="' + pathg + 'resources/images/select.gif" />');
        $('#' + val).find('span').addClass('activ');
        $('#loading').show();
        this.findall(1, $('#ModulId').val());
        this.resizesection();
        $('#contentactions').slideUp('fast');
    };

    this.binddetail2 = function (val) {
        $('#SectionId').val(val != 0 ? val : null);
        $('#conttreecontent').find('.cll').remove();
        $('#conttreecontent').find('.activ').removeClass('activ');
        $('#' + val + ' > nobr > img').after('<img class="cll" src="' + pathg + 'resources/images/select.gif" />');
        $('#' + val).find('span').addClass('activ');
        $('#loading').show();
        this.findall(1, $('#ModulId').val());
        this.resizesection();
        $('#contentactions').slideUp('fast');
    };

    this.findall = function (pag, mod) {
        $('#idPage').val(pag);
        var findname = ($('#txname').val() != '' && $('#txname').val() != this.TextFind) ? $('#txname').val() : undefined;
        var me = this;
        $.post(this.UrlGetContent, { mod: mod, page: pag, sectionId: $('#SectionId').val(), active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), name: findname }, function (data) {
            $('#listado').html(data.html);
            $('#idCount').val(data.count);
            $('#spnnumregis').html(data.count);
            $('#spnresult').html(data.total);
            $('#loading').hide();
            if (data.total < 8) {
                $('#dvpaginator').hide();
            }
            else {
                $('#dvpaginator').show();
            }
            if (data.count == 0) {
                $('#listado').append('<li>' + me.NotFound + '</li>');
            }
            me.draggable();
        });
        $('#contentactions').slideUp('fast');
    };

    this.resizesection = function () {
        if ($('#contctncontent').height() > 522)
            $('#conttreecontent').css('height', $('#contctncontent').height());
        else
            $('#conttreecontent').css('height', 522);
    };

    this.cleanall = function () {
        $('#ddlautor').val(null);
        $('#ddlstatus').val(null);
        $('#ddlfilter').val(null);
        $('#txname').val('Buscar por nombre de contenido');
        $('#conttree').find('.cll').remove();
        $('#SectionId').val(null);
        this.findall(1);
    };

    this.deleteall = function () {
        var idcontent = new Array();
        $('input:checkbox').each(function (index) {
            if (this.checked) {
                idcontent[idcontent.length] = this.value;
            }
        });

        if (idcontent.length > 0)
            this.deletecontent(idcontent.join(','));
        else
            alerts('Debes seleccionar por lo menos un contenido para eliminar');
    };

    this.deletecontent = function (val) {
        $.post(this.UrlDeleteContent, { listContentId: val }, function (data) {
            if (data != null && data.result) {
                var strid = val.split(',');
                for (var i = 0; i < strid.length; i++) {
                    $('#li' + strid[i]).remove();
                    $('#contentactions').slideUp('fast');
                }
            }

            var count = parseInt($('#idCount').val()) - strid.length;
            var total = parseInt($('#spnresult').html()) - strid.length;
            $('#idCount').val(count);
            $('#spnnumregis').html(count);
            $('#spnresult').html(total);
        });
    };

    this.newcontent = function (msg) {
        var SectionId = $('#SectionId').val();
        if (SectionId != null && SectionId != undefined) {            
            window.location.href = this.UrlNewContent + '&sectionId=' + SectionId;
        } else {
            alerts(msg);
        }
    };

    this.contentselect = function (obj, id) {
        if ($(obj).is('.liact')) {
            $(obj).removeClass('liact');
            $('#contentactions').slideUp('fast');
            $('#contentid').val(null);
        }
        else {
            $('.liact').removeClass('liact');
            $(obj).addClass('liact');
            $('#contentactions').slideDown('fast');
            $('#contentid').val(id);
        }
    };

    this.editcontent = function (val) {
        window.location.href = this.UrlDetailContent + '&id=' + val;
    };

    this.addClone = function (id, title, msg) {
        $.post(pathg + 'Admin/Content/AddClone', { id: id }, function (data) {
            if (data.result) {
                if (document.getElementById('dvClone') == null) {
                    $('body').append('<div id="dvClone"><img onclick="ctnback.CloneContent(\'' + msg + '\')" title="' + title + '" src="' + pathg + 'Resources/Images/clone.png"><span>' + data.numclone + '</span></div>');
                } else {
                    $('#dvClone > span').html(data.numclone);
                }
            }
        });
    };

    this.CloneContent = function (msg) {
        var SectionId = $('#SectionId').val();
        if (SectionId != null && SectionId != undefined && SectionId != '') {
            $.post(pathg + 'Admin/Content/CloneContent', { sectionId: SectionId }, function (data) {
                if (data.result) {
                    $('#dvClone').remove();
                    ctnback.binddetail(SectionId);
                }
            });
        } else {
            alerts(msg);
        }
    };
};
/* end helpers content */
/* helpers section */
sectionclass = function () {
    this.UrlChangeParent = pathg + 'Admin/Secciones/ChangeParent';
    this.UrlDetail = pathg + 'Admin/Secciones/Detail';
    this.UrlSaveSection = pathg + 'Admin/Secciones/SaveSection';
    this.UrlGetChild = pathg + 'Admin/Secciones/GetChild';
    this.UrlDelete = pathg + 'Admin/Secciones/DeleteSection';

    this.resizable = function () {
        $("#conttree").resizable({
            maxWidth: 640,
            minWidth: 240
        });
    };

    this.funcinicializate = function () {
        var url = this.UrlChangeParent;
        var me = this;
        $('#conttree div').droppable({
            hoverClass: "sectionactive",
            drop: function (event, ui) {
                var parentId = $(this).attr('id');
                var sectionId = ui.draggable.parent().parent().attr('id');
                if (parentId != sectionId) {
                    $(ui.draggable.parent().parent().attr('id')).remove();
                    ui.draggable.parent().parent().hide();
                    $.post(url, { sectionId: sectionId, parentId: parentId }, function (data) {
                        if (data != null && data.result) {
                            $('#conttree').html(data.html);
                            initialize();
                            me.resizable();
                        }
                    });
                }
            }
        });

        $('#conttree span').draggable({
            revert: "invalid",
            autoSize: true,
            ghosting: false,
            axis: "y"
        });
    };

    this.binddetail = function (val) {
        this.removeall();
        $('#fseccion')[0].reset();
        $('#imgdel').show();
        $('#liorder').show();
        $('#conttree').find('.cll').remove();
        $('#conttree').find('.activ').removeClass('activ');
        $('#' + val + ' > nobr > img').after('<img class="cll" src="' + pathg + 'resources/images/select.gif" />');
        $('#loading').show();
        $.post(this.UrlDetail, { id: val }, function (data) {
            if (data != null) {
                $('#SectionId').val(data.SectionId);
                $('#Name').val(data.Name);
                $('#Description').val(data.Description);
                $('#Sectiontype').val(data.Sectiontype);
                $('#Target').val(data.Target);
                if (data.Active)
                    $('#Active').attr("checked", "checked");
                if (data.Private)
                    $('#Private').attr("checked", "checked");
                if (data.Url)
                    $('#Url').attr("checked", "checked");
                $('#NavigateUrl').val(data.Navigateurl);
                $('#Sectionorder').val(data.Sectionorder);
                $('#OldOrder').val(data.Sectionorder);
                $('#MaxValue').val(data.MaxValue);
                $('#Layout').val(data.Layout);
                $('#Template').val(data.Template);
                $('#ParentId').val(data.ParentId);
                $('#Friendlyname').val(data.Friendlyname);
                $('#loading').hide();
                fadesec();
                $('#imup').show();
                $('#imdown').show();
                $('#' + val).find('span').addClass('activ');
            }
        });
    };

    this.savesection = function () {
        if (this.validatesection()) {
            var str = $('#NavigateUrl').val() != '' ? "'" + $('#NavigateUrl').val() + "'" : null;
            var parentid = $('#ParentId').val() != '' && $('#ParentId').val() != undefined ? $('#ParentId').val() : null;
            var SectionId = $('#SectionId').val() != '' && $('#SectionId').val() != undefined ? $('#SectionId').val() : null;
            var oldorder = $('#OldOrder').val() != '' && $('#OldOrder').val() != undefined ? parseInt($('#OldOrder').val()) : null;
            var sectionorder = $('#Sectionorder').val() != '' && $('#Sectionorder').val() != undefined ? $('#Sectionorder').val() : null;

            var active = ($('#Active').attr('checked') ? true : false);
            var private = ($('#Private').attr('checked') ? true : false);
            var url = ($('#Url').attr('checked') ? true : false);
            var friendly = $('#Friendlyname').val() != '' ? "'" + $('#Friendlyname').val() + "'" : null;

            var source = "{ SectionId: " + SectionId + ", ParentId: " + parentid + ", Name: '" + $('#Name').val() +
                "', Description: '" + $('#Description').val() + "', Layout: '" + $('#Layout').val() +
                "', Template: '" + $('#Template').val() + "', Sectiontype: '" + $('#Sectiontype').val() +
                "', Target: '" + $('#Target').val() +
                "', Active: " + active + ", Private: " + private +
                ", Url: " + url +
                ", Navigateurl: " + str +
                ", Sectionorder: " + sectionorder + ", Friendlyname: " + friendly + ", OldOrder: " + oldorder + " }";
            $.ajax({
                type: 'POST',
                url: this.UrlSaveSection,
                data: source,
                success: function (request) { window.location.href = window.location.href; },
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            });
        }
    };

    this.validatesection = function () {
        var result = true;
        $('.val').each(function (index) {
            if ($(this).val() == undefined || $(this).val() == '') {
                $(this).addClass('input-validation-error');
                result = false;
            }
        });
        return result;
    };

    this.removeall = function () {
        $('.input-validation-error').each(function (index) {
            $(this).removeClass('input-validation-error');
        });
    };

    this.newsection = function (IsRoot) {
        this.removeall();
        var idsec = $('#SectionId').val();
        $('#fseccion')[0].reset();
        $('#ParentId').val(idsec);
        if (IsRoot) {
            $('#conttree').find('.cll').remove();
            $('#0').append('<img class="cll" src="' + pathg + 'resources/images/select.gif" />');
        }
        $('#SectionId').val(null);
        $('#imgdel').hide();
        $('#liorder').hide();
        fadesec();
    };

    this.changeord = function (value) {
        var val = parseInt($('#Sectionorder').val());
        var valmax = parseInt($('#MaxValue').val());
        val += value;
        if (val != 0 && val != valmax)
            $('#Sectionorder').val(val);
    };

    this.deletesection = function () {
        var sectionid = $('#SectionId').val();
        $.post(this.UrlDelete, { sectionId: sectionid }, function (data) {
            if (data != null && data.result) {
                $(document).find('#' + sectionid).parent().remove();
                backsec();
            }
        });
    };
};
/* end helpers section */

var map;
var marker;
var contentType = "";

function hideTemplates(content) {
    switch (content) {
        case 'challenge':
            $('#content-template').hide();
            $('#IContent_Template').val('Challenge');
            break;
        case 'question':
            $('#content-template').hide();
            $('#IContent_Template').val('Question');
            break;
        case 'blog':
            $('#IContent_Template option[value="Challenge"]').remove();
            $('#IContent_Template option[value="Question"]').remove();
            //$('#IContent_Private').parent().hide();
            break;
    }
}

/* Question section */
function initializeMapQuestion() {
    var mapOptions = {
        center: new google.maps.LatLng(6.230833, -75.590556),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
    marker = new google.maps.Marker();
    google.maps.event.addListener(map, 'click', function (event) {
        marker.setMap(null);
        marker = new google.maps.Marker({
            map: map,
            position: event.latLng
        });
        $('#Question_XCoordinate').val(event.latLng.lng().toString().replace('.', ','));
        $('#Question_YCoordinate').val(event.latLng.lat().toString().replace('.', ','));
    });
}

function changeQuestionType() {
    var type = $('#Question_Type').val();
    $('#minimumQuestions').remove();
    switch (type) {
        case 'Seleccion_Multiple':
            $('#questionAnswers').show();
            $('#questionLocation').hide();
            break;
        case 'Ubicacion':
            $('#questionAnswers').hide();
            $('#questionLocation').show();
            var center = map.getCenter();
            google.maps.event.trigger(map, "resize");
            map.setCenter(center);
            break;
        case 'Abierta':
            $('#questionAnswers').hide();
            $('#questionLocation').hide();
            break;
    }
}

function questionAddAnswer() {
    var li = bc_newElement('li');
    $(li).html('<table><tr><th>Imagen</th><th>Texto</th><th></th></tr></table>');

    var answerAmount = $('.fileanswer').size();
    var fileInput = bc_newElement('input');
    fileInput.type = 'file';
    fileInput.id = 'answer-' + answerAmount;
    fileInput.name = 'fileanswer';
    fileInput.className = 'fileanswer';
    fileInput.maxLength = '150';
    $(fileInput).css('width', '150px');
    //addEvent(inp, 'blur', bc_verif);

    var img = bc_newElement('IMG');
    img.className = 'del'
    img.src = pathg + 'resources/images/35del.gif';
    img.align = 'right';
    img.width = '35';
    img.height = '35';
    img.title = 'Eliminar respuesta';
    addEvent(img, 'click', questionRemoveAnswer);

    var ico = bc_newElement('img');
    ico.align = 'left';
    ico.src = pathg + 'resources/images/image.gif';

    var textInput = bc_newElement('input');
    textInput.type = 'text';
    textInput.name = 'txtanswer';
    textInput.maxLength = '250';
    textInput.className = 'text';

    var tr = bc_newElement('tr');
    var td1 = bc_newElement('td');
    var td2 = bc_newElement('td');
    var td3 = bc_newElement('td');

    td1.appendChild(fileInput);
    td2.appendChild(textInput);
    td3.appendChild(img);
    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    $(li).children('table').append(tr);
    $('#questionAnswers ul').append(li);
}

function questionRemoveAnswer(event) {
    $(this).closest('li').remove();
}
/* End Question section */

/* Challenge section */
function initializeMapChallenge() {
    var mapOptions = {
        center: new google.maps.LatLng(6.230833, -75.590556),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
    marker = new google.maps.Marker();
    google.maps.event.addListener(map, 'click', function (event) {
        marker.setMap(null);
        marker = new google.maps.Marker({
            map: map,
            position: event.latLng
        });
        $('#Challenge_XCoordinate').val(event.latLng.lng().toString().replace('.',','));
        $('#Challenge_YCoordinate').val(event.latLng.lat().toString().replace('.', ','));
    });
}

function changeChallengeType() {
    var type = $('#Challenge_Type').val();
    switch (type) {
        case 'Participacion_Ciudadana':
            $('#challengePeople').show();
            break;
        case 'Desafio_Ciudad':
            $('#challengePeople').hide();
            break;
    }
}
/* End challenge section */

/* Custom validators section */
$.validator.addMethod("dategreaterthan", function (value, element, params) {
    return Date.parse(value) > Date.parse($(params).val());
});

$.validator.unobtrusive.adapters.add("dategreaterthan", ["otherpropertyname"], function (options) {
    options.rules["dategreaterthan"] = "#" + contentType + "_" + options.params.otherpropertyname;
    options.messages["dategreaterthan"] = options.message;
});
/* End Custom validators section */

/* Idea section */
function initializeMapIdea() {
    var mapOptions = {
        center: new google.maps.LatLng(6.230833, -75.590556),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
    marker = new google.maps.Marker();
    google.maps.event.addListener(map, 'click', function (event) {
        marker.setMap(null);
        marker = new google.maps.Marker({
            map: map,
            position: event.latLng
        });
        $('#Idea_XCoordinate').val(event.latLng.lng().toString().replace('.', ','));
        $('#Idea_YCoordinate').val(event.latLng.lat().toString().replace('.', ','));
    });
}

function editComments() {
    $('.edit').click(function () {
        $(this).parents('.comment').children('.commentValue').hide();
        $(this).parents('.comment').children('.commentForm').show();
    });
    $('.cancel').click(function () {
        $(this).parents('.comment').children('.commentValue').show();
        $(this).parents('.comment').children('.commentForm').hide();
    });
    $('.save').click(function () {
        var text = $(this).siblings('.commentText').val();
        var id = $(this).parents('li').children('.commentId').val();
        var save = $(this);
        $.ajax({
            type: 'POST',
            url: pathg + 'Admin/Idea/UpdateComment',
            data: { id: id, text: text }
        })
        .done(function (request) {
            if (request.result) {
                save.parents('.comment').children('.commentValue').show();
                save.parents('.comment').children('.commentValue span').html(text);
                save.parents('.comment').children('.commentForm').hide();
            }
        });
    });
    $('.block').click(function () {
        var id = $(this).parents('li').children('.commentId').val();
        var block = $(this);
        $.ajax({
            type: 'POST',
            url: pathg + 'Admin/Idea/BlockComment',
            data: { id: id }
        })
        .done(function (request) {
            if (request.result) {
                block.hide();
                block.siblings('.unblock').show();
            }
        });
    });
    $('.unblock').click(function () {
        var id = $(this).parents('li').children('.commentId').val();
        var unblock = $(this);
        $.ajax({
            type: 'POST',
            url: pathg + 'Admin/Idea/UnBlockComment',
            data: { id: id }
        })
        .done(function (request) {
            if (request.result) {
                unblock.hide();
                unblock.siblings('.block').show();
            }
        });
    });
    $('#morecomments').click(function () {
        commentsPagingIdea();
    });
    if (commentsPageSize > totalIdeaComments) {
        $('#morecomments').hide();
    }
}

function ideapaging(mod) {
    var UrlGetIdeas = pathg + 'Admin/Idea/GetIdeas';
    var findtext = ($('#txname').val() != '' && $('#txname').val() != ctnback.TextFind) ? $('#txname').val() : undefined;
    var pag = parseInt($('#idPage').val());
    var total = parseInt($('#idCount').val());
    var result = parseInt($('#spnresult').html());

    var me = ctnback;
    if (total != result) {
        $('#loading').show();
        pag++;
        $.post(UrlGetIdeas, { mod: mod, page: pag, active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), text: findtext }, function (data) {
            $('#listado').append(data.html);
            $('#idCount').val(total + data.count);
            $('#spnnumregis').html($('#idCount').val());
            $('#loading').hide();
            $('#idPage').val(pag);
            me.draggable();
            me.resizesection();
            if (data.count == 0) {
                $('#listado').append('<li>' + me.NotFound + '</li>');
            }
        });
    }
};

function ideafindall(pag, mod) {
    var UrlGetIdeas = pathg + 'Admin/Idea/GetIdeas';
    $('#idPage').val(pag);
    var findtext = ($('#txname').val() != '' && $('#txname').val() != ctnback.TextFind) ? $('#txname').val() : undefined;
    var me = ctnback;
    $.post(UrlGetIdeas, { mod: mod, page: pag, active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), text: findtext }, function (data) {
        $('#listado').html(data.html);
        $('#idCount').val(data.count);
        $('#spnnumregis').html(data.count);
        $('#spnresult').html(data.total);
        $('#loading').hide();
        if (data.total < 8) {
            $('#dvpaginator').hide();
        }
        else {
            $('#dvpaginator').show();
        }
        if (data.count == 0) {
            $('#listado').append('<li>' + me.NotFound + '</li>');
        }
        me.draggable();
    });
    $('#contentactions').slideUp('fast');
};

function ideacleanall() {
    $('#ddlautor').val(null);
    $('#ddlstatus').val(null);
    $('#ddlfilter').val(null);
    $('#txname').val('Buscar por palabra o frase');
    $('#conttree').find('.cll').remove();
    $('#SectionId').val(null);
    ideafindall(1, 55);
};

function commentsPagingIdea() {
    commentsPageIndex++;
    if (commentsPageIndex * commentsPageSize > totalIdeaComments) {
        $('#morecomments').hide();
    }
    $.ajax({
        type: 'POST',
        dataType: 'html',
        url: pathg + 'Admin/idea/Comments',
        data: {
            ideaId: ideaId,
            pageIndex: commentsPageIndex,
            pageSize: commentsPageSize
        }
    }).done(function (html) {
        if (html.trim().length > 0) {
            $('#comments-list-container').append(html);
            $('#commentspage').html($('.comment').length);
        }
        else {
            if (pageIndex == 1) {
                systemAlert('Oh no!!', 'No tenemos ningun resultado con la información que nos pediste :(');
                $('#comments-list-container').empty();
            }
        }
    });
}
/* End Idea section */

/* Idea report section */

function ideasReports() {
    $('.block-idea').unbind('click');
    $('.block-idea').click(function () {
        blockIdea($(this));
    });
    $('.unblock-idea').unbind('click');
    $('.unblock-idea').click(function () {
        unblockIdea($(this));
    });
    $('.block-idea-user').unbind('click');
    $('.block-idea-user').click(function () {
        blockUser($(this));
    });
    $('.unblock-idea-user').unbind('click');
    $('.unblock-idea-user').click(function () {
        unblockUser($(this));
    });
    $('.check-report').unbind('click');
    $('.check-report').click(function () {
        reportChecked($(this));
    });
}

function reportChecked(elem) {
    var id = elem.attr("data-id");
    $.ajax({
        type: 'POST',
        url: pathg + 'Admin/IdeaReport/IdeaReportChecked',
        data: { id: id }
    })
    .done(function (request) {
        if (request.result) {
            elem.replaceWith('<img src="'+pathg+'resources/images/25check.png" />');
        }
    });
}

function blockIdea(elem) {
    var id = elem.parent().attr("data-id");
    $.ajax({
        type: 'POST',
        url: pathg + 'Admin/IdeaReport/BlockIdea',
        data: { id: id }
    })
    .done(function (request) {
        if (request.result) {
            elem.siblings('.unblock-idea').show();
            elem.hide();
        }
    });
}

function unblockIdea(elem) {
    var id = elem.parent().attr("data-id");
    $.ajax({
        type: 'POST',
        url: pathg + 'Admin/IdeaReport/UnBlockIdea',
        data: { id: id }
    })
    .done(function (request) {
        if (request.result) {
            elem.siblings('.block-idea').show();
            elem.hide();
        }
    });
}

function unblockUser(elem) {
    var id = elem.parent().attr("data-id");
    $.ajax({
        type: 'POST',
        url: pathg + 'Admin/IdeaReport/UnBlockUser',
        data: { id: id }
    })
    .done(function (request) {
        if (request.result) {
            elem.siblings('.block-idea-user').show();
            elem.hide();
        }
    });
}

function blockUser(elem) {
    var id = elem.parent().attr("data-id");
    $.ajax({
        type: 'POST',
        url: pathg + 'Admin/IdeaReport/BlockUser',
        data: { id: id }
    })
    .done(function (request) {
        if (request.result) {
            elem.siblings('.unblock-idea-user').show();
            elem.hide();
        }
    });
}

function ideareportpaging(mod) {
    var UrlGetIdeas = pathg + 'Admin/IdeaReport/GetIdeaReports';
    var findtext = ($('#txname').val() != '' && $('#txname').val() != ctnback.TextFind) ? $('#txname').val() : undefined;
    var pag = parseInt($('#idPage').val());
    var total = parseInt($('#idCount').val());
    var result = parseInt($('#spnresult').html());

    var me = ctnback;
    if (total != result) {
        $('#loading').show();
        pag++;
        $.post(UrlGetIdeas, { mod: mod, page: pag, active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), text: findtext }, function (data) {
            $('#listado').append(data.html);
            $('#idCount').val(total + data.count);
            $('#spnnumregis').html($('#idCount').val());
            $('#loading').hide();
            $('#idPage').val(pag);
            me.draggable();
            me.resizesection();
            if (data.count == 0) {
                $('#listado').append('<tr><td colspan="6">' + me.NotFound + '</td></tr>');
            }
        });
    }
};

function ideareportfindall(pag, mod) {
    var UrlGetIdeas = pathg + 'Admin/IdeaReport/GetIdeaReports';
    $('#idPage').val(pag);
    var findtext = ($('#txname').val() != '' && $('#txname').val() != ctnback.TextFind) ? $('#txname').val() : undefined;
    var me = ctnback;
    $.post(UrlGetIdeas, { mod: mod, page: pag, active: $('#ddlstatus').val(), filter: $('#ddlfilter').val(), text: findtext }, function (data) {
        $('#listado').html(data.html);
        $('#idCount').val(data.count);
        $('#spnnumregis').html(data.count);
        $('#spnresult').html(data.total);
        $('#loading').hide();
        if (data.total < 8) {
            $('#dvpaginator').hide();
        }
        else {
            $('#dvpaginator').show();
        }
        if (data.count == 0) {
            $('#listado').append('<tr><td colspan="6">' + me.NotFound + '</td></tr>');
        }
        me.draggable();
    });
    $('#contentactions').slideUp('fast');
};

function ideareportcleanall() {
    $('#ddlautor').val(null);
    $('#ddlstatus').val(null);
    $('#ddlfilter').val(null);
    $('#txname').val('Buscar por palabra o frase');
    $('#conttree').find('.cll').remove();
    $('#SectionId').val(null);
    ideafindall(1, 56);
};

/// <summary>
/// Updates the count.
/// </summary>
function updateCount() {
    var max = $(this).attr('maxlength');
    var t = $(this).val();
    var e = t.match(/(\r\n|\n|\r)/g);
    var a = 0;
    if (e != null) {
        a = e.length;
    }

    var cs = max - ($(this).val().length + a);
    var span = $(this).parent().find("span.character");
    span.text("Max. " + cs + " characters");
}
/* End Idea report section */
var pathMM = 'http://mimedellin.dasigno.com';

var widgetMM = {
    init: function (data) {
        if (data.id) {
            if (data.width) {
                data.width = data.width * 1;
                data.width = data.width < 300 ? 300 : data.width > 460 ? 460 : data.width;
            }
            else {
                data.width = 300;
            }

            if (data.height) {
                data.height = data.height * 1;
                data.height = data.height < 400 ? 400 : data.height > 615 ? 615 : data.height;
            }
            else {
                data.height = 300;
            }

            var $frame = $('<iframe scrolling="no" />')
            $frame.attr('src', pathMM + '/widget/?width=' + data.width + '&height=' + data.height)
              .css('width', data.width)
              .css('height', data.height)
              .css('overflow', 'hidden');
            $('#' + data.id).prepend($frame);
        }
    }
}

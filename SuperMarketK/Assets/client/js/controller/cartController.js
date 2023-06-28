let cartController = function() {
    // Xóa item trong giỏ hàng
    $('.btn-danger').off('click').on('click', function () {
        $.ajax({
            data: { id: $(this).data('id') },
            url: '/Cart/Delete',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/Cart";
                }
            }
        })
    });

    $('.value-plus').on('click', function () {
        var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) + 1;
        divUpd.text(newVal);
        var idThis = $('#' + $(this).data('id'));
        $.ajax({
            data: { id: $(this).data('id') },
            url: '/Cart/UpdateUp',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                var labelTienHang = $("#tienHang");
                var tien = labelTienHang.text().replaceAll(',', '');
                var html = format2(parseInt(tien.substring(0, tien.indexOf('VNĐ'))) + res.priceItem) + '<span> VNĐ</span>';
                labelTienHang.html(html);

                var labelTongTien = $("#tongTien");
                tien = labelTongTien.text().replaceAll(',', '');
                var htmlTongTien = format2(parseInt(tien.substring(0, tien.indexOf('VNĐ'))) + res.priceItem) + '<span> VNĐ</span>';
                labelTongTien.html(htmlTongTien);

                idThis.text(res.priceItem * newVal);
            }
        })
    });

    $('.value-minus').on('click', function () {
        var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) - 1;
        if (newVal >= 1) divUpd.text(newVal);
        var idThis = $('#' + $(this).data('id'));
        $.ajax({
            data: { id: $(this).data('id') },
            url: '/Cart/UpdateDown',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                var labelTienHang = $("#tienHang");
                var tien = labelTienHang.text().replaceAll(',', '');
                var html = format2(parseInt(tien.substring(0, tien.indexOf('VNĐ'))) - res.priceItem) + '<span> VNĐ</span>';
                labelTienHang.html(html);

                var labelTongTien = $("#tongTien");
                tien = labelTongTien.text().replaceAll(',', '');
                var htmlTongTien = format2(parseInt(tien.substring(0, tien.indexOf('VNĐ'))) - res.priceItem) + '<span> VNĐ</span>';
                labelTongTien.html(htmlTongTien);

                idThis.text(res.priceItem * newVal);
            }
        })
    });
}
cartController();

function format2(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
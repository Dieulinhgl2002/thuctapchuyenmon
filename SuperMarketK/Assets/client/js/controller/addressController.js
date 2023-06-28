var address = {
    init: function () {
        address.loadProvince();
        address.event();
    },
    event: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                address.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');
            }
        });

        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                address.loadPhuongXa(parseInt(id));
            }
            else {
                $('#ddlWard').html('');
            }
        });

        $('#ddlWard').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                var districtID = parseInt($('#ddlDistrict').val());
                address.loadTienShip(parseInt(id), districtID);
            }
        });

    },
    loadProvince: function () {
        var html = '';
        $.ajax({
            url: '/Cart/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Chọn tỉnh thành --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.ID + '">' + item.Name + ' </option>'
                    });
                    $('#ddlProvince').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        var html = '';
        $.ajax({
            url: '/Cart/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Chọn quận huyện --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.ID + '">' + item.Name + ' </option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadPhuongXa: function (id) {
        var html = '';
        $.ajax({
            url: '/Cart/LoadPhuongXa',
            type: "POST",
            data: { districtID: id },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Chọn phường xã --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.WardCode + '">' + item.WardName + ' </option>'
                    });
                    $('#ddlWard').html(html);
                }
            }
        })
    },
    loadTienShip: function (id, districtID) {
        var html = '';
        $.ajax({
            url: '/Cart/getTienShip',
            type: "POST",
            data: { wardCode: id, districtID: districtID },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var label = $("#tienShip");
                    var html = format2(responce.data) + '<span> VNĐ</span>';
                    var labelTongTien = $("#tongTien");
                    var labelTienHang = $("#tienHang");
                    var tongTien = responce.data + parseInt(labelTienHang.text().substring(0, labelTienHang.text().indexOf('VNĐ')))*1000;
                    var htmlTongTien = format2(tongTien) + '<span> VNĐ</span>';
                    labelTongTien.html(htmlTongTien);
                    label.html(html);
                }
            }
        })
    }
}
address.init();

function format2(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
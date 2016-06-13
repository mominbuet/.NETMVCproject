$(document).ready(function () {
    var uname_for_auto = "";
    var uid_for_auto = "";

    $.ui.autocomplete.prototype._renderMenu = function (ul, items) {
        var self = this;
        //table definitions
        ul.append('<table class="table table-responsive autoc"><thead></thead><tbody></tbody></table>');
        $.each(items, function (index, item) {
            self._renderItemData(ul, ul.find("table tbody"), item);
        });
        ul.find("table thead").prepend('<tr class="ui-menu-item"><th>UserName</th><th>FullName</th><th>Cell</th><th>Desig</th><th>Division</th><th>Zilla</th><th>Thana</th><th>Institute</th></tr>');
    };
    $.ui.autocomplete.prototype._renderItemData = function (ul, table, item) {
        return this._renderItem(table, item).data("ui-autocomplete-item", item);
    };
    $.ui.autocomplete.prototype._renderItem = function (table, item) {
        return $("<tr class='ui-menu-item' ></tr>")
        .data("item.autocomplete", item)
        .append("<td >" + item.username + "</td><td >" + item.fullname + "</td><td >" + item.cell + "</td><td>" +
            item.designation + "</td><td >" + item.division + "</td><td >" + item.zilla + "</td><td>" + item.thana + "</td><td>" + item.institute + "</td>")
        .appendTo(table);
    };

    $("#txtZM").autocomplete({

        minLength: 3,
        source: function (req, response) {
            $.getJSON("/DCR/SearchUser?cell=" + req.term, function (data) {
                response($.map(data, function (item) {
                    //console.log(data);
                    return {

                        username: item.username,
                        id: item.id,
                        fullname: item.fullname,
                        division: item.division,
                        zilla: item.zilla,
                        thana: item.thana,
                        cell: item.cell,
                        designation: item.designation,
                        institute: item.institute
                    };
                }));
            });
        },
        /*hover: function (event, ui) {
            
        return false;
        },*/

        select: function (event, ui) {
            //console.log(ui.item);
            console.log('hover');
            $("#txtZM").val(ui.item.username);
            //$("#USER_NO").val(ui.item.id);
            //$("#Search_User").val(ui.item.id);
            return false;
        },
        focus: function (event, ui) {
            //console.log(ui.item.id);
            uname_for_auto = ui.item.fullname;
            uid_for_auto = ui.item.id;
            //$("#txtZM").val(ui.item.fullname);
            //$("#USER_NO").val(ui.item.id);
            $("#Search_User").val(ui.item.id);
            return false;
        }
    });

    $("#txtZM").change(function () {
        if ($("#txtZM").val().length == 0) {
            $("#USER_NO").val("");
            $("#Search_User").val("");
        }
    });
    $("#txtZM").bind('autocompleteselect', function (event, ui) {
        console.log(ui.item);
    });
    $('.ui-autocomplete table tbody tr').live('click', function () {
        $('.ui-autocomplete').css('display', 'none');
        $("#txtZM").val(uname_for_auto);
        $("#USER_NO").val(uid_for_auto);
        uname_for_auto = "";
        uid_for_auto = "";
    });
    //alert('23');
    $('.ui-autocomplete table tbody tr').live('hover', function () {
        //alert('23');
        $('.ui-autocomplete table tbody tr').each(function () {

            $(this).removeClass('ui-state-focus2');
        });
        $(this).addClass('ui-state-focus2');

    });

});

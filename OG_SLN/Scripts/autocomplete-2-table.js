var which_one = 0;

var zm_fullname = '';
var zm_username = '';
var zm_mobile = '';
var zm_designation = '';
var zm_division = '';
var zm_zilla = '';
var zm_thana = '';

var ins_name = '';
var ins_code = '';
var ins_division = '';
var ins_zilla = '';
var ins_thana = '';

$(document).ready(function () {
    var uname_for_auto = "";
    var uname_for_auto2 = "";
    var uid_for_auto = "";
    var uid_for_auto2 = "";

    $.ui.autocomplete.prototype._renderMenu = function (ul, items) {
        var self = this;
        //table definitions
        ul.append('<table class="table table-responsive autoc"><thead></thead><tbody></tbody></table>');
        $.each(items, function (index, item) {
            self._renderItemData(ul, ul.find("table tbody"), item);
        });
        if (which_one == 0)
            ul.find("table thead").prepend('<tr class="ui-menu-item"><th>UserName</th><th>FullName</th><th>Cell</th><th>Desig</th><th>Division</th><th>Zilla</th><th>Thana</th><th>Institute</th></tr>');
        else
            ul.find("table thead").prepend('<tr class="ui-menu-item"><th>Institute</th><th>Code</th><th>Division</th><th>Zilla</th><th>Thana</th></tr>');

    };
    $.ui.autocomplete.prototype._renderItemData = function (ul, table, item) {
        return this._renderItem(table, item).data("ui-autocomplete-item", item);
    };
    $.ui.autocomplete.prototype._renderItem = function (table, item) {
        if (which_one == 0)
            return $("<tr class='ui-menu-item' ></tr>")
            .data("item.autocomplete", item)
            .append("<td >" + item.username + "</td><td >" + item.fullname + "</td><td >" + item.cell + "</td><td>" +
                item.designation + "</td><td >" + item.division + "</td><td >" + item.zilla + "</td><td>" + item.thana + "</td><td>" + item.institute + "</td>")
            .appendTo(table);
        else
            return $("<tr class='ui-menu-item' ></tr>")
            .data("item.autocomplete", item)
            .append("<td >" + item.institute + "</td><td >" + item.inscode + "</td><td >"
            + item.division + "</td><td >" + item.zilla + "</td><td>" + item.thana + "</td>")
            .appendTo(table);
    };

    $("#txtZM").autocomplete({
        minLength: 3,
        source: function (req, response) {
            $.getJSON("/DCR/SearchUser?cell=" + req.term, function (data) {
                response($.map(data, function (item) {
                    which_one = 0;
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

        select: function (event, ui) {
            $("#txtZM").val(ui.item.username);
            return false;
        },
        focus: function (event, ui) {
            uname_for_auto = ui.item.fullname;
            uid_for_auto = ui.item.id;
            which_one = 0;
            $("#Search_User").val(ui.item.id);

            zm_fullname = ui.item.fullname;
            zm_username = ui.item.username;
            zm_mobile = ui.item.cell;
            zm_designation = ui.item.designation;
            zm_division = ui.item.division;
            zm_zilla = ui.item.zilla;
            zm_thana = ui.item.thana;

            return false;
        }
    });

    $("#txtZM").change(function () {
        if ($("#txtZM").val().length == 0) {
            $("#USER_NO").val("");
            $("#Search_User").val("");

            $('#zm_res').html('');
        }
    });
    $("#txtZM").bind('autocompleteselect', function (event, ui) {
        
    });
    $('.ui-autocomplete table tbody tr').live('click', function () {
        $('.ui-autocomplete').css('display', 'none');
        if (which_one == 0) {
            $("#txtZM").val(uname_for_auto);
            $("#USER_NO").val(uid_for_auto);

            uname_for_auto = "";
            uid_for_auto = "";
        }
        else {
            $("#txtINS").val(uname_for_auto2);
            $("#INSTITUTE_NO").val(uid_for_auto2);

            uname_for_auto2 = "";
            uid_for_auto2 = "";
        }
    });

    $('.ui-autocomplete table tbody tr').live('hover', function () {
        $('.ui-autocomplete table tbody tr').each(function () {
            $(this).removeClass('ui-state-focus2');
        });
        $(this).addClass('ui-state-focus2');

    });

    $("#txtINS").autocomplete({
        minLength: 3,
        source: function (req, response) {
            if ($('#USER_NO').val() != '') {
                $.getJSON("/QuestionProject/SearchInstitute?id=" + $('#USER_NO').val() + "&cell=" + req.term, function (data) {
                    response($.map(data, function (item) {
                        which_one = 1;
                        return {
                            institute: item.institute,
                            inscode: item.inscode,
                            division: item.division,
                            zilla: item.zilla,
                            zilla: item.zilla,
                            thana: item.thana
                        };
                    }));
                });
            }
            else {
                alert('Please select Zonal Manager first');
            }
        },

        select: function (event, ui) {
            $("#txtINS").val(ui.item.institute);
            return false;
        },
        focus: function (event, ui) {
            uname_for_auto2 = ui.item.institute;
            uid_for_auto2 = ui.item.inscode;
            which_one = 1;
            $("#Search_User").val(ui.item.id);

            ins_name = ui.item.institute;
            ins_code = ui.item.inscode;
            ins_division = ui.item.division;
            ins_zilla = ui.item.zilla;
            ins_thana = ui.item.thana;

            return false;
        }
    });

    $("#txtINS").change(function () {
        if ($("#txtINS").val().length == 0) {
            $("#INSTITUTE_NO").val("");
            which_one = 1;

            $('#ins_res').html('');
        }
    });
});

$(function () {

    layer.config({
        extend: 'extend/layer.ext.js'
    });

    $("img.lazy").lazyload({
        threshold: 200
    });

    $(".site-nav-item-mega").hoverIntent(
	  function () {
	      $(this).find(".mega-nav").stop().slideToggle();
	  }
	);


    $(".site-my-account").hoverIntent(
	  function () {
	      $(this).find(".nav-user-backend").stop().slideToggle();
	  }
	);

    $(".shop-nav .shop-cat").hoverIntent(
      function () {
          $(this).find(".shop-cat-list").stop().slideToggle();
      }
    );

    $(".shop-nav ul li").hover(
	  function () {
	      $(this).toggleClass("selected");
	  }
	);

    $(".shop-item .item-product").hover(function () {
        $(this).find(".actionbox").stop().fadeToggle("fast");
    });

    $(".product-item .product-img").hover(function () {
        $(this).find(".actionbox").stop().fadeToggle("fast");
    });

    $(".site-nav .shop-publish").click(function () {
        shopAddClick();
    });

    $(".js-redirect-to").click(function () {
        rediretTo($(this).attr("itemid"));
    });

    if ($("#hlgo").length > 0) {
        blockRedirectUI();
        window.setInterval("skipTo()", 50);
    }

    setCatSelectedCssForProductList();
    shopProductToggleComment();
    postShopComment();
    contactEmailHandler();
    //retinaHandler();

    //操作提示框：
    layerOpen("alert-error", 2);
    layerOpen("alert-success", 1);
    layerOpen("alert-warning", 0);

    $(".site-account div a").height($(".site-nav").height() - 24);

    hrefToLower();

    $(".site-header .site-search").click(function () {
        openSearchBox();
    });

    $(".site-nav li.mobile-search-icon").click(function () {
        openSearchBox();
    });



});

var layerOpen = function (classType, iconNumber) {

    if ($(".alert." + classType).length > 0) {
        if ($.trim($(".alert." + classType)[0].innerHTML).length > 0) {
            layer.open({
                content: $("#" + classType).html(),
                icon: iconNumber
            });
        }
    }
}

var hrefToLower = function () {
    $("a").each(function () {
        if ($(this).attr("href") != undefined && !$(this).hasClass("out-link")) $(this).attr("href", $(this).attr("href").toLowerCase());
    });
}

$.fn.enterKey = function (fnc) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == 13) {
                fnc.call(this, ev);
            }
        })
    })
}

var contactEmailHandler = function () {
    $("#contact-email").text(GlobalSetting.ContactEmail);
}

var retinaHandler = function () {
    if (window.devicePixelRatio > 1.5) {
        $("#logo-img").attr("src", "/image/logo@2x.png")
    }
}

var openSearchBox = function () {
    layer.open({
        type: 1,
        shade: false,
        title: false, //不显示标题
        area: ['95%', '95%'],
        content: $('.search-box'), //捕获的元素
        cancel: function (index) {
            layer.close(index);
        }
    });
}

$('.cart-btn').on('click', function () {
    if($(window).width() > 1200){
        var cart = $('#sitebar-drawar');
    }else{
        var cart = $('.footer-cart');
    }
    var imgtodrag = $(this).parents('.product-item').find(".product-thumb img").eq(0);
    if (imgtodrag) {
        var imgclone = imgtodrag.clone()
            .offset({
                top: imgtodrag.offset().top,
                left: imgtodrag.offset().left
            })
            .css({
                'opacity': '0.5',
                'position': 'absolute',
                'height': '150px',
                'width': '150px',
                'z-index': '100'
            })
            .appendTo($('body'))
            .animate({
                'top': cart.offset().top + 10,
                'left': cart.offset().left + 10,
                'width': 35,
                'height': 35
            }, 1000, 'easeInOutExpo');
        setTimeout(function () {
            cart
        }, 1500);

        imgclone.animate({
            'width': 0,
            'height': 0
        }, function () {
            $(this).detach()
        });
    }
});
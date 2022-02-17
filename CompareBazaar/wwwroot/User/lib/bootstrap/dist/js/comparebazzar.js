$('#owl-carousel1').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    // dots:false,

    responsive: {
        200: {
            items: 2
        },
        300: {
            items: 3
        },
        400: {
            items: 4
        },
        500: {
            items: 5
        },
        600: {
            items: 6
        },

        900: {
            items: 7
        },
        1000: {
            items: 8
        },

        1100: {
            items: 9
        },
        1200: {
            items: 10
        },
        1300: {
            items: 11
        },
        1400: {
            items: 12
        }
    }
})
$('#owl-carousel2').owlCarousel({
    loop: true,
    margin: 15,
    nav: false,
    autoplay: true,
    autoplayTimeout: 2000,
    responsive: {

        0: {
            items: 1
        },

        900: {
            items: 2
        },
        1000: {
            items: 2
        },
        1300: {
            items: 3
        },
        1400: {
            items: 5
        }
    }
})
function Wishlist(wishlist) {
    if (wishlist.className != 'text-black') {
        wishlist.className = 'text-black';
    }
    else {
        wishlist.className = 'text-red';
    }
}

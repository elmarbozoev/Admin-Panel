

  new Swiper('.swiper__hero', {
    // Optional parameters
    direction: 'horizontal',
    loop: true,
    slidesPerView: 1,
    autoplay: {
      delay: 8000,
    },
    // If we need pagination
    pagination: {
      el: '.swiper-pagination',
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
  });


  new Swiper('.swiper__news', {
    // Optional parameters
    direction: 'horizontal',
    loop: true,
    slidesPerView: 4,
  
    // Navigation arrows
    autoplay: {
      delay: 3000,
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
  
 
  });
  
  new Swiper('.swiper__proj', {
    // Optional parameters
    direction: 'horizontal',
    loop: true,
    slidesPerView: 3,
    autoplay: {
      delay: 5000,
    },
    spaceBetween:70,
    // Navigation arrows
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
  
 
  });
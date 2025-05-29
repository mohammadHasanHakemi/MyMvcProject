// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// tagggles


const navbarToggle = document.querySelector('.navbar-toggle');
const navbarMenu = document.querySelector('.navbar-menu');

navbarToggle.addEventListener('click', () => {
    navbarToggle.classList.toggle('active');
    navbarMenu.classList.toggle('active');
});


// buttons
const links = document.querySelectorAll(".lamp");
console.log(links);

const clickHandler = (link) => {
    links.forEach((link) => {
        link.classList.remove("active")
    })
    link.classList.add("active")
    // console.log("sasdasds");
}

links.forEach((link) => {
    link.addEventListener("click" , () => clickHandler(link))
})
// ابتدا همه عناصر اصلی را مخفی می‌کنیم
document.querySelector('.conteiner').style.display = 'none';
document.querySelector('footer').style.display = 'none';
document.querySelector('.container-abu').style.display = 'none';

// منتظر می‌مانیم تا تمام منابع صفحه لود شوند
window.onload = function() {
    // 3 ثانیه (3000 میلی‌ثانیه) صبر کنید
    setTimeout(function() {
        // نمایش عناصر اصلی
        document.querySelector('.conteiner').style.display = 'block';
        document.querySelector('footer').style.display = 'flex';
        document.querySelector('.container-abu').style.display = 'flex';
        
        // مخفی کردن لودر
        document.querySelector('.loader').style.display = 'none';
    }, 30); // زمان تأخیر 3 ثانیه
};


  //slider
  const prev = document.querySelector(".prev");
const next = document.querySelector(".next");

next.addEventListener('click', function () {
    let items = document.querySelectorAll('.item');
    let prop = document.querySelectorAll('.prop-one-Album');
    document.querySelector('.cards').appendChild(items[0]);
    document.querySelector('.propertyAlbums').appendChild(prop[0]);

})
prev.addEventListener('click', function () {
    let items = document.querySelectorAll('.item');
    let prop = document.querySelectorAll('.prop-one-Album');
    
    document.querySelector('.cards').prepend(items[items.length - 1]);
    document.querySelector('.propertyAlbums').prepend(prop[prop.length - 1]);

})
//searchbar
// script.js
document.getElementById('searchInput').addEventListener('input', function() {
    const searchTerm = this.value.toLowerCase();
    const h3Elements = document.querySelectorAll('.content h3');
    const resultsContainer = document.getElementById('resultsContainer');
    resultsContainer.innerHTML = '';
    if (searchTerm.trim() === '') {
        resultsContainer.style.display = 'none'; // اگر سرچبار خالی باشد، نتایج مخفی شود
        return;
    }
    else{
        resultsContainer.style.display = 'flex';
    }
    h3Elements.forEach(function(h3) {
        console.log('Found h3:', h3, 'ID:', h3.id);
        const h3Text = h3.textContent.toLowerCase();
        if (h3Text.includes(searchTerm)) {
            const resultItem = document.createElement('a'); // استفاده از <a> به جای <div>
            resultItem.textContent = h3.textContent;
            resultItem.href = `#${h3.id}`; // لینک به id مربوطه
             // نمایش هر لینک در یک خط جدید
            resultItem.style.margin = '5px 0'; // فاصله بین لینک‌ها
            resultItem.style.color = 'rgb(252, 193, 0.5)'; // رنگ لینک
            resultItem.style.textDecoration = 'none'; // حذف زیرخط لینک
            resultsContainer.appendChild(resultItem);
        }
    });
});
// const cards = document.querySelectorAll(".item");
// cards.forEach((sd) => {
//     sd.classList.remove("hide")
// })
// console.log(cards);

// for (let index = 0; index < 2; index++) {
//     cards[index].classList.add('hide')    
// }
// for (let index = 5; index < cards.length; index++) {
//     cards[index].classList.add('hide')    
// }




//music player
document.addEventListener('DOMContentLoaded', function () {
    
    const audio = document.getElementById('audio');

    const playPauseButton = document.getElementById('play-pause');
    const stopButton = document.getElementById('stop');
    const volumeControl = document.getElementById('volume');
    const songButtons = document.querySelectorAll('.song');
    const posterContainer = document.getElementById('poster');
    const timeline = document.getElementById('timeline');
    const currentTimeDisplay = document.getElementById('current-time');
    const durationDisplay = document.getElementById('duration');
    const text = document.querySelector('.text');
    const Album = document.querySelector('.prop');
    

    let currentActiveButton = null; // دکمه آهنگ فعال فعلی

    // هندل کلیک روی دکمه اصلی پلی لیست
    songButtons.forEach(songBtn => {
        
        songBtn.addEventListener('click', function () {
                        document.querySelectorAll('.songcontainer').forEach(container => {
                container.classList.remove('musicactive');
            });
            const container = songBtn.closest('.songcontainer');
            const songSrc = this.getAttribute('data-src');

            const isSameSong = audio.src.includes(songSrc);

            const posterSrc = this.getAttribute('data-poster');
            const textsrc = this.getAttribute('data-name');
            const propsrc = this.getAttribute('data-album');

            // دکمه img مربوط به همین موزیک
            const thisPlayBtn = container.querySelector('.imgplay');
            const thisGif = container.querySelector('.gifplay');
            container.classList.add('musicactive');
            // همه دکمه‌ها رو ریست کن (حتی اگه آهنگ یکی باشه)
            document.querySelectorAll('.imgplay').forEach(img => img.src = 'Images/icons/play2.png');

            if (!isSameSong) {
                // اگه موزیک متفاوت بود، دیتای جدید لود بشه
                audio.src = songSrc;
                posterContainer.src = posterSrc;
                text.textContent = textsrc;
                Album.textContent = propsrc;

                audio.play();

                document.getElementById('imgPlay').src = 'Images/icons/paused.png';
                 thisGif.src = "Images/Mplay.gif";
                 thisPlayBtn.src = 'Images/icons/play1.png';

                currentActiveButton = this; // ذخیره آهنگ فعال

            } else {
                // اگه آهنگ فعلیه، فقط پلی/پاز بشه
                if (audio.paused) {
                    audio.play();
                    document.getElementById('imgPlay').src = 'Images/icons/paused.png';
                     thisGif.src = "Images/Mplay.gif";
                    thisPlayBtn.src = 'Images/icons/play1.png';
                } else {
                    audio.pause();
                    document.getElementById('imgPlay').src = 'Images/icons/play.png';
                    thisGif.src = "Images/Mplay.png";
                     thisPlayBtn.src = 'Images/icons/play2.png';
                }
            }
        });
    });
playPauseButton.addEventListener('click',function(){
                if (audio.paused) {
                    audio.play();
                    document.getElementById('imgPlay').src = 'Images/icons/paused.png';
                    if (thisGif) thisGif.src = "Images/Mplay.gif";
                    if (thisPlayBtn) thisPlayBtn.src = 'Images/icons/play1.png';
                } else {
                    audio.pause();
                    document.getElementById('imgPlay').src = 'Images/icons/play.png';
                    if (thisGif) thisGif.src = "Images/Mplay.png";
                    if (thisPlayBtn) thisPlayBtn.src = 'Images/icons/play2.png';
                }
});
    // دکمه توقف کامل
    stopButton.addEventListener('click', function () {
        audio.pause();
        audio.currentTime = 0;
        document.getElementById('imgPlay').src = 'Images/icons/play.png';
        if (currentActiveButton) {
            const gif = currentActiveButton.querySelector('.gifplay');
            const img = currentActiveButton.querySelector('.imgplay');
            if (gif) gif.src = "Images/Mplay.png";
            if (img) img.src = 'Images/icons/play2.png';
        }
    });

    // کنترل صدا
    volumeControl.addEventListener('input', function () {
        audio.volume = volumeControl.value;
    });

    // به‌روزرسانی تایم‌لاین
    audio.addEventListener('timeupdate', function () {
        const currentTime = audio.currentTime;
        const duration = audio.duration;
        timeline.value = (currentTime / duration) * 100;
        currentTimeDisplay.textContent = formatTime(currentTime);
        if (!isNaN(duration)) durationDisplay.textContent = formatTime(duration);
    });

    // کلیک روی تایم‌لاین
    timeline.addEventListener('input', function () {
        const seekTime = (timeline.value / 100) * audio.duration;
        audio.currentTime = seekTime;
    });

    // فرمت زمان
    function formatTime(time) {
        const minutes = Math.floor(time / 60);
        const seconds = Math.floor(time % 60);
        return `${minutes}:${seconds.toString().padStart(2, '0')}`;
    }
});

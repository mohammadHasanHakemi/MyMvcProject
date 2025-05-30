// تولید یا گرفتن deviceId از localStorage
function getDeviceId() {
    let deviceId = localStorage.getItem('deviceId');
    if (!deviceId) {
        // تولید UUID ساده (برای تولید UUID می‌تونی از کتابخونه uuid هم استفاده کنی)
        deviceId = 'device-' + Math.random().toString(36).substr(2, 9);
        localStorage.setItem('deviceId', deviceId);
    }
    return deviceId;
}

// فرستادن درخواست View به API
async function recordView(musicId) {
    const deviceId = getDeviceId();
    try {
        const response = await fetch(`http://localhost:5062/api/music/${musicId}/view`, {
            method: 'POST',
            headers: {
                'deviceId': deviceId
            }
        });
        if (!response.ok) {
            throw new Error('Failed to record view');
        }
        const data = await response.json();
        console.log('View recorded:', data);
    } catch (error) {
        console.error('Error recording view:', error);
    }
}

// فرستادن درخواست Like به API
async function toggleLike(musicId) {
    const deviceId = getDeviceId();
    try {
        const response = await fetch(`http://localhost:5062/api/music/${musicId}/like`, {
            method: 'POST',
            headers: {
                'deviceId': deviceId
            }
        });
        if (!response.ok) {
            throw new Error('Failed to toggle like');
        }
        const data = await response.json();
        console.log('Like toggled:', data);
    } catch (error) {
        console.error('Error toggling like:', error);
    }
}

// گرفتن وضعیت (View و Like) به‌صورت real-time
async function updateStats(musicId) {
    const deviceId = getDeviceId();
    try {
        const response = await fetch(`http://localhost:5062/api/music/${musicId}/status`, {
            headers: {
                'deviceId': deviceId
            }
        });
        if (!response.ok) {
            throw new Error('Failed to get status');
        }
        const data = await response.json();
        // آپدیت DOM با تعداد View و Like
        document.getElementById(`views-${musicId}`).innerText = data.views;
        document.getElementById(`likes-${musicId}`).innerText = data.likes;
        // آپدیت وضعیت checkbox
        const likeCheckbox = document.getElementById(`like-checkbox-${musicId}`);
        if (likeCheckbox) {
            likeCheckbox.checked = data.isLiked; // تیک checkbox رو بر اساس isLiked تنظیم کن
        }
    } catch (error) {
        console.error('Error updating stats:', error);
    }
}

// Polling برای آپدیت real-time هر 5 ثانیه
function startPolling(musicId) {
    updateStats(musicId); // آپدیت اولیه
    setInterval(() => updateStats(musicId), 50); // هر 5 ثانیه آپدیت
}

// موقع لود صفحه، برای هر موزیک polling و event listenerها رو تنظیم کن
document.addEventListener('DOMContentLoaded', () => {
    // پیدا کردن همه دکمه‌های Play (برای پیدا کردن musicIdها)
    const playButtons = document.querySelectorAll('[id^="play-btn-"]');
    playButtons.forEach(button => {
        const musicId = button.id.replace('play-btn-', ''); // استخراج musicId از id دکمه
        startPolling(musicId); // شروع Polling برای آپدیت View و Like

        // اضافه کردن event listener برای دکمه Play
        const playBtn = document.getElementById(`play-btn-${musicId}`);
        if (playBtn) {
            playBtn.addEventListener('click', () => {
                recordView(musicId); // ثبت View موقع کلیک روی Play
            });
        }

        // اضافه کردن event listener برای checkbox Like
        const likeCheckbox = document.getElementById(`like-checkbox-${musicId}`);
        if (likeCheckbox) {
            likeCheckbox.addEventListener('change', () => {
                toggleLike(musicId); // ثبت/حذف Like موقع تغییر checkbox
            });
        }
    });
});
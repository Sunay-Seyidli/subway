mergeInto(LibraryManager.library, {
    IsMobileDevice: function() {
        var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        return isMobile ? 1 : 0;
    },
    GetDevicePixelRatio: function() {
        return window.devicePixelRatio || 1;
    },
    GetScreenWidth: function() {
        return window.screen.width;
    },
    GetScreenHeight: function() {
        return window.screen.height;
    }
});

const storageKey = "theme";

export function Initialize() {
    // applyStoredTheme();

    document.addEventListener("DOMContentLoaded", applyStoredTheme);

    if (window.Blazor?.addEventListener) {
        window.Blazor.addEventListener("enhancedload", applyStoredTheme);
    }
}
export function setTheme(theme) {
    setHtmlThemeAttribute('data-bs-theme', theme);
}

export function setHtmlThemeAttribute(attribute, value) {
    document.documentElement.setAttribute(attribute, value);
}

export function getTheme() {
    return getHtmlThemeAttribute('data-bs-theme');
}

export function getHtmlThemeAttribute(attribute) {
    return document.documentElement.getAttribute(attribute);
}

export function setStoredTheme(theme) {
    localStorage.setItem(storageKey, theme);
}

export function getStoredTheme() {
    return localStorage.getItem(storageKey);
}

// Detects current system theme: "light" or "dark"
export function getSystemTheme() {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

export function applyStoredTheme() {
    const theme = getStoredTheme() || getSystemTheme();
    setTheme(theme);
}

// Allows .NET to subscribe to theme changes
export function registerThemeChangeHandler(dotNetObjRef) {
    const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

    mediaQuery.addEventListener("change", e => {
        if (getStoredTheme()) {
            return;
        }

        const theme = e.matches ? "dark" : "light";
        setTheme(theme);
        dotNetObjRef.invokeMethodAsync("OnThemeChanged", theme);
    });
}

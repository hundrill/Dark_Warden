# 정림사지

[배포처 바로가기](https://www.buyeofont.kr/jrsjfont)

&nbsp;

## 웹 폰트

사용하는 `font-family`의 이름은 `Jeongnimsaji`입니다.

### HTML

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji.css" type="text/css"/>
```

### CSS `@Import`

```css
@import url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji.css');
```

### CSS `@font-face`

```css
@font-face {
    font-family: 'Jeongnimsaji';
    font-weight: 400;
    font-style: normal;
    font-display: swap;
    src: url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Regular.woff2') format('woff2'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Regular.woff') format('woff'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Regular.otf') format('opentype'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Regular.ttf') format('truetype');
}
@font-face {
    font-family: 'Jeongnimsaji';
    font-weight: 700;
    font-style: normal;
    font-display: swap;
    src: url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Bold.woff2') format('woff2'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Bold.woff') format('woff'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Bold.otf') format('opentype'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/Jeongnimsaji-Bold.ttf') format('truetype');
}
```

&nbsp;

## 다이나믹 서브셋

웹폰트의 최적화를 위해 모던 브라우저에서는 글리프를 여러개로 나누어 필요한 부분만 동적으로 파싱하는 다이나믹 서브셋을 제공합니다. 폰트의 용량이 부담된다면 아래 코드를 사용하는 걸 추천합니다.

### HTML

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/subsets/Jeongnimsaji-dynamic-subset.css" type="text/css"/>
```

### CSS

```css
@import url('https://cdn.jsdelivr.net/gh/fonts-archive/Jeongnimsaji/subsets/Jeongnimsaji-dynamic-subset.css');
```

&nbsp;

## font-family

어느 브라우저나 시스템 환경에서도 동일한 폰트가 적용되어야 한다면 아래와 같이 구성하는 걸 추천합니다. `-apple-system`과 `BlinkMacSystemFont`는 맥, `Segoe UI`는 윈도우, `Roboto`는 안드로이드의 기본 폰트입니다.


```css
font-family: "Jeongnimsaji", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
```

&nbsp;

## 라이선스

라이선스는 언제든지 변경될 수 있습니다. 변경사항을 확인하려면 배포처를 방문해 주세요.

```
정림사지 서체, 신동엽 손글씨 서체는 개인 및 기업 사용자를 포함, 모든 사용자에게 무료로 제공되며 글꼴 자체를 유료로 판매하는 것을 제외한 상업적인 사용이 가능합니다. 정확한 사용 조건은 라이센스 전문을 참고하시기를 바랍니다.
정림사지 서체,신동엽 손글씨 서체는 오픈 라이센스 글꼴로 동일한 저작권 규정을 적용받고 있으며, 글꼴을 사용한 인쇄물, 광고물(온라인 포함)의 부여군청, 부여군 지역공동체 활성화재단의 홍보를 위해 활용될 수 있습니다.
이를 원치 않는 사용자는 언제든지 당사에 요청하실 수 있습니다. 서체 웹 사이트는 부여군 지역공동체 활성화재단이 운영합니다.
```

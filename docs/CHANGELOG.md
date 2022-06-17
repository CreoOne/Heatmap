# Changes

What was released in specific version and what not, but is worked on.

&nbsp;

---

&nbsp;

## Unreleased changes

_nothing_

&nbsp;

---

&nbsp;

## 0.0.4-rc

### Change

- NuGet's pushed in Release configuration instead of Debug

&nbsp;

---

&nbsp;

## 0.0.3-rc

### Addition

- NuGet's contain license information

&nbsp;

---

&nbsp;

## 0.0.1-rc

### Fix

- Downgraded SkiaSharp to 2.80.2 from 2.88.x because it produces corrupted output when working with FileStreams [original issue](https://github.com/mono/SkiaSharp/issues/1962).

### Addition

- `PreCachedGradient` that allows for faster color retireval by cache lookup at the small off-by-one cost in precision.
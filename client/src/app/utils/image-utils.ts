import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

export function getSafeImageUrl
(sanitizer: DomSanitizer, imageBytes: Uint8Array | null): SafeUrl | null {
  if (imageBytes && imageBytes.length > 0) {
    const base64String = btoa(String.fromCharCode(...imageBytes));
    const dataUrl = `data:image/jpeg;base64,${base64String}`;
    return sanitizer.bypassSecurityTrustUrl(dataUrl);
  }
  return null;
}

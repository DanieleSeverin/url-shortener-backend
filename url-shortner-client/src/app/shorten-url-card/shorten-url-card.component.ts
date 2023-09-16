import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-shorten-url-card',
  templateUrl: './shorten-url-card.component.html',
  styleUrls: ['./shorten-url-card.component.scss']
})
export class ShortenUrlCardComponent {

  shortenedUrl?: string;

  form = this._fb.group({
    link: ['', {
      validators: [Validators.required, Validators.pattern('https?://.+')]
    }]
  });

  get link() { return this.form.get('link');}

  constructor(private _fb: FormBuilder, private _api: ApiService) { }

  shorten(){
    this._api.post('shorten', {url: this.link?.value}).subscribe({
      next: (res : { url: string }) => {
        console.log(res);
        this.link?.reset();
        this.shortenedUrl = res.url;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  copyToClipboard(){
    if(this.shortenedUrl){
      navigator.clipboard.writeText(this.shortenedUrl);
      alert("Copied to clipboard: " + this.shortenedUrl);
    }
  }

}
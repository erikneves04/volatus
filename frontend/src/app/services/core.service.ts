import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class CoreService {
    private apiUrl: string;

    constructor(private http: HttpClient) {
        // Use environment variable or default to localhost
        this.apiUrl = environment.apiUrl || 'http://localhost:8081';
    }

    getOptions() {
        return this.optionsSignal();
    }

    setOptions(options: Partial<AppSettings>) {
        this.optionsSignal.update((current) => ({
            ...current,
            ...options,
        }));
    }

}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  constructor(private http: HttpClient) { }

  getMapsByUid(mapUidList: string[]): Observable<any> {
    const params = { mapUidList: mapUidList.join(',') };
    return this.http.get<any>('https://prod.trackmania.core.nadeo.online/maps', { params });
  }

  getMapsById(mapIdList: string[]): Observable<any> {
    const params = { mapIdList: mapIdList.join(',') };
    return this.http.get<any>('https://prod.trackmania.core.nadeo.online/maps', { params });
  }
}

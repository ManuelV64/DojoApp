import { TestBed } from '@angular/core/testing';

import { LifeGameHttpService } from './life-game-http.service';

describe('LifeGameHttpService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LifeGameHttpService = TestBed.get(LifeGameHttpService);
    expect(service).toBeTruthy();
  });
});

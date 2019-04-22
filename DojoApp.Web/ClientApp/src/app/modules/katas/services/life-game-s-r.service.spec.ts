import { TestBed } from '@angular/core/testing';

import { LifeGameSRService } from './life-game-s-r.service';

describe('LifeGameSRService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LifeGameSRService = TestBed.get(LifeGameSRService);
    expect(service).toBeTruthy();
  });
});

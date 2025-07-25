import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MdoalConfirmationComponent } from './mdoal-confirmation.component';

describe('MdoalConfirmationComponent', () => {
  let component: MdoalConfirmationComponent;
  let fixture: ComponentFixture<MdoalConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MdoalConfirmationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MdoalConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

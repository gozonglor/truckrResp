$("#pay_method").change(function()
{
  $('.payment_method').slideUp();
  switch($('#pay_method :selected').val())
  {
    case '1': $('#cash_payment').slideDown(); break;
    case '2': $('#credit_card_payment').slideDown(); break;
    case '3': $('#debit_card_payment').slideDown(); break;
    case '4': $('#netbanking_payment').slideDown(); break;
  }
});